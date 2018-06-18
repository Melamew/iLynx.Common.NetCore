using System;

namespace iLynx.UI.Sfml.Animation
{
    public abstract class Animation : IAnimation
    {
        private readonly double duration;
        private readonly LoopMode loopMode;
        private readonly Func<double, double> easingFunction = d => d;

        protected Animation(TimeSpan duration, LoopMode loopMode = LoopMode.None, Func<double, double> easingFunction = null)
        {
            this.duration = duration.TotalMilliseconds;
            this.loopMode = loopMode;
            this.easingFunction = easingFunction ?? this.easingFunction;
        }

        public void Tick(TimeSpan elapsed)
        {
            var elapsedMs = elapsed.TotalMilliseconds;
            var timeIndex = elapsedMs % duration;
            var end = (int)Math.Floor(elapsedMs / duration) % 2;
            switch (loopMode)
            {
                case LoopMode.None when end == 1:
                    IsFinished = true;
                    break;
                case LoopMode.Restart when end == 1:
                    end = 0;
                    break;
            }
            timeIndex = Math.Abs(end - timeIndex / duration);
            Tick(easingFunction(timeIndex));
        }

        protected abstract void Tick(double timeIndex);

        public bool IsFinished { get; private set; }
    }
}