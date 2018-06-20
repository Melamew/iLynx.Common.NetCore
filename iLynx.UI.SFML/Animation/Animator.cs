#region LICENSE
/*
 * Copyright 2018 Melanie Hjorth
 *
 * Redistribution and use in source and binary forms,
 * with or without modification,
 * are permitted provided that the following conditions are met:
 *
 * 1. Redistributions of source code must retain the above copyright notice,
 * this list of conditions and the following disclaimer.
 *
 * 2. Redistributions in binary form must reproduce the above copyright notice,
 * this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
 *
 * 3. Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote
 * products derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
 * INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
 * HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
 * OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE,
 * EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 */
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using iLynx.Common.Threading;

namespace iLynx.UI.Sfml.Animation
{
    public static class Animator
    {
        private static Thread animationThread;
        private static readonly Dictionary<IAnimation, DateTime> Animations = new Dictionary<IAnimation, DateTime>();
        private static volatile bool isRunning;
        private static TimeSpan frameInterval;
        private static double desiredFramerate = 60d;
        // ReSharper disable once InconsistentNaming
        private static readonly ReaderWriterLockSlim rwl = new ReaderWriterLockSlim();

        static Animator()
        {
            frameInterval = TimeSpan.FromSeconds(1d / desiredFramerate);
            StartAnimationThread();
        }

        public static void StartAnimationThread()
        {
            if (isRunning) return;
            isRunning = true;
            animationThread = new Thread(DoAnimations) { IsBackground = true };
            animationThread.Start();
        }

        public static void StopAnimationThread()
        {
            if (!isRunning) return;
            isRunning = false;
            animationThread.Join();
        }

        public static IAnimation AddAnimation(IAnimation animation)
        {
            try
            {
                rwl.EnterWriteLock();
                Animations.Add(animation, DateTime.Now);
                return animation;
            }
            finally
            {
                rwl.ExitWriteLock();
            }
        }

        public static IAnimation RemoveAnimation(IAnimation animation)
        {
            try
            {
                rwl.EnterWriteLock();
                return Animations.Remove(animation) ? animation : null;
            }
            finally
            {
                rwl.ExitWriteLock();
            }
        }

        private static async void DoAnimations(object state)
        {
            var lastCleanup = DateTime.Now;
            while (isRunning)
            {
                KeyValuePair<IAnimation, DateTime>[] anims;
                try
                {
                    rwl.EnterReadLock();
                    anims = Animations.ToArray();
                }
                finally
                {
                    rwl.ExitReadLock();
                }
                foreach (var animation in anims.Where(x => !x.Key.IsFinished))
                    await Task.Run(() => animation.Key.Tick(DateTime.Now - animation.Value));
                Thread.CurrentThread.Join(frameInterval);
                if (DateTime.Now - lastCleanup < CleanupInterval) continue;
                try
                {
                    lastCleanup = DateTime.Now;
                    rwl.EnterWriteLock();
                    foreach (var finished in anims.Where(x => x.Key.IsFinished))
                        Animations.Remove(finished.Key);
                }
                finally
                {
                    rwl.ExitWriteLock();
                }
            }
        }

        public static double DesiredFramerate
        {
            get => desiredFramerate;
            set
            {
                if (Math.Abs(value - desiredFramerate) <= double.Epsilon) return;
                desiredFramerate = value;
                frameInterval = TimeSpan.FromMilliseconds(1000d / desiredFramerate);
            }
        }

        public static TimeSpan CleanupInterval { get; set; } = TimeSpan.FromMilliseconds(250d);
    }
}