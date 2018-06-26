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
        private static readonly Dictionary<IAnimation, DateTime> Animations = new Dictionary<IAnimation, DateTime>();
        private static readonly ReaderWriterLockSlim Rwl = new ReaderWriterLockSlim();
        private static DateTime lastCleanup = DateTime.Now;
        private static readonly CallbackTicker Ticker = new CallbackTicker();

        static Animator()
        {
            StartAnimationThread();
        }

        public static void StartAnimationThread()
        {
            lastCleanup = DateTime.Now;
            Ticker.Start(DoAnimations);
        }

        public static void StopAnimationThread()
        {
            Ticker.Stop();
        }

        public static IAnimation AddAnimation(IAnimation animation)
        {
            try
            {
                Rwl.EnterWriteLock();
                Animations.Add(animation, DateTime.Now);
                return animation;
            }
            finally
            {
                Rwl.ExitWriteLock();
            }
        }

        public static IAnimation RemoveAnimation(IAnimation animation)
        {
            try
            {
                Rwl.EnterWriteLock();
                return Animations.Remove(animation) ? animation : null;
            }
            finally
            {
                Rwl.ExitWriteLock();
            }
        }

        private static void DoAnimations()
        {
            KeyValuePair<IAnimation, DateTime>[] anims;
            try
            {
                Rwl.EnterReadLock();
                anims = Animations.ToArray();
            }
            finally
            {
                Rwl.ExitReadLock();
            }

            foreach (var animation in anims)
            {
                if (animation.Key.IsFinished)
                {
                    Rwl.EnterWriteLock();
                    foreach (var finished in anims.Where(x => x.Key.IsFinished))
                        Animations.Remove(finished.Key);
                    Rwl.ExitWriteLock();
                    continue;
                }
                animation.Key.Tick(DateTime.Now - animation.Value);
            }
        }

        public static IAnimation Start(Action<double> callback, TimeSpan duration, LoopMode loopMode = LoopMode.None, Func<double, double> easingFunction = null)
        {
            return AddAnimation(new CallbackAnimation(callback, duration, loopMode, easingFunction));
        }

        public static void Stop(IAnimation animation)
        {
            if (null == animation) return;
            animation.Cancel();
            while (Animations.ContainsKey(animation))
                Thread.CurrentThread.Join(1);
            animation.Tick(animation.Duration);
        }
    }
}