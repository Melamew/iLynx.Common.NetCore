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

        private static void DoAnimations(object state)
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
                    animation.Key.Tick(DateTime.Now - animation.Value);
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