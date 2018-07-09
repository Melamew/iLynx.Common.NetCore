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

namespace iLynx.Graphics.Animation
{
    /// <summary>
    /// Defines a method for animating various things
    /// </summary>
    public interface IAnimator
    {
        /// <summary>
        /// Adds the specified <see cref="IAnimation"/> to this <see cref="IAnimator"/>.
        /// </summary>
        /// <param name="animation">The animation to add</param>
        /// <returns></returns>
        IAnimation AddAnimation(IAnimation animation);
        /// <summary>
        /// Removes the specified <see cref="IAnimation"/> from this <see cref="IAnimator"/>
        /// </summary>
        /// <param name="animation"></param>
        /// <returns></returns>
        IAnimation RemoveAnimation(IAnimation animation);
        /// <summary>
        /// Processes one "tick" - All animations will be invoked once with this call.
        /// </summary>
        void Tick();
        /// <summary>
        /// Stops the specified <see cref="IAnimation"/> and removes it from this <see cref="IAnimator"/>
        /// </summary>
        /// <param name="animation"></param>
        void Stop(IAnimation animation);
    }

    public static class ExtensionMethods
    {
        public static IAnimation Start(this IAnimator animator, Action<double> callback, TimeSpan duration, LoopMode loopMode = LoopMode.None, Func<double, double> easingFunction = null)
        {
            return animator.AddAnimation(new CallbackAnimation(callback, duration, loopMode, easingFunction));
        }
    }

    public class Animator : IAnimator
    {
        private readonly Dictionary<IAnimation, DateTime> m_animations = new Dictionary<IAnimation, DateTime>();
        private readonly ReaderWriterLockSlim m_rwl = new ReaderWriterLockSlim();

        public IAnimation AddAnimation(IAnimation animation)
        {
            try
            {
                m_rwl.EnterWriteLock();
                m_animations.Add(animation, DateTime.Now);
                return animation;
            }
            finally
            {
                m_rwl.ExitWriteLock();
            }
        }

        public IAnimation RemoveAnimation(IAnimation animation)
        {
            try
            {
                m_rwl.EnterWriteLock();
                return m_animations.Remove(animation) ? animation : null;
            }
            finally
            {
                m_rwl.ExitWriteLock();
            }
        }

        public void Tick()
        {
            KeyValuePair<IAnimation, DateTime>[] anims;
            try
            {
                m_rwl.EnterReadLock();
                anims = m_animations.ToArray();
            }
            finally
            {
                m_rwl.ExitReadLock();
            }

            foreach (var animation in anims)
            {
                if (animation.Key.IsFinished)
                {
                    m_rwl.EnterWriteLock();
                    m_animations.Remove(animation.Key);
                    m_rwl.ExitWriteLock();
                    continue;
                }
                animation.Key.Tick(DateTime.Now - animation.Value);
            }
        }

        /// <inheritdoc/>
        public void Stop(IAnimation animation)
        {
            if (null == animation) return;
            animation.Cancel();
            while (m_animations.ContainsKey(animation))
                Thread.CurrentThread.Join(1);
            animation.Tick(animation.Duration);
        }
    }
}