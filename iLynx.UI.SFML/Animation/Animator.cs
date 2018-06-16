using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using iLynx.Common.Threading;

namespace iLynx.UI.Sfml.Animation
{
    public class Animator
    {
        private readonly Thread animationThread;
        private readonly TimeSpan cleanupInterval;
        private readonly List<(DateTime, IAnimation)> animations = new List<(DateTime, IAnimation)>();
        private volatile bool isRunning;
        private double desiredFramerate;
        private TimeSpan frameInterval;
        public Animator(double desiredFramerate = 60d, double cleanupIntervalMs = 250d, bool startThread = false)
        {
            this.desiredFramerate = desiredFramerate;
            frameInterval = TimeSpan.FromSeconds(1d / desiredFramerate);
            cleanupInterval = TimeSpan.FromMilliseconds(cleanupIntervalMs);
            animationThread = new Thread(DoAnimations) { IsBackground = true };
            if (startThread)
                StartAnimator();
        }

        public void StartAnimator()
        {
            if (isRunning) StopAnimator();
            isRunning = true;
            animationThread.Start();
        }

        public void StopAnimator()
        {
            if (!isRunning) return;
            isRunning = false;
            animationThread.Join();
        }

        public void Start(IAnimation animation)
        {
            lock (animations)
                animations.Add((DateTime.Now, animation));
        }

        private void DoAnimations(object state)
        {
            var lastCleanup = DateTime.Now;
            
            while (isRunning)
            {
                (DateTime, IAnimation)[] runningAnimations;
                lock (animations)
                    runningAnimations = animations.Where(x => !x.Item2.IsFinished).ToArray();
                Parallel.ForEach(runningAnimations,
                    animation => { animation.Item2.Tick(DateTime.Now - animation.Item1); });
                Thread.CurrentThread.Join(frameInterval);
                if (DateTime.Now - lastCleanup < cleanupInterval) continue;
                lock (animations)
                {
                    animations.RemoveAll(x => x.Item2.IsFinished);
                    lastCleanup = DateTime.Now;
                }
            }
        }

        public double DesiredFramerate
        {
            get => desiredFramerate;
            set
            {
                if (Math.Abs(value - desiredFramerate) <= double.Epsilon) return;
                desiredFramerate = value;
                frameInterval = TimeSpan.FromSeconds(1d / desiredFramerate);
            }
        }
    }
}