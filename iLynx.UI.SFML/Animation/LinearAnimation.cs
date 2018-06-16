using System;
using iLynx.Common;

namespace iLynx.UI.Sfml.Animation
{
    public class LinearAnimation<T> : IAnimation
    {
        private readonly IBinding<T> target;
        private readonly LoopMode loopMode;
        private dynamic start;
        private dynamic end;
        private readonly TimeSpan duration;
        private dynamic deltaPerMs;
        private TimeSpan loopOffset = TimeSpan.Zero;

        public LinearAnimation(T start, T end, TimeSpan duration, IBinding<T> target, LoopMode loopMode = LoopMode.None)
        {
            this.start = start;
            this.end = end;
            this.duration = duration;
            this.target = target;
            this.loopMode = loopMode;
            CalculateDelta();
        }

        private void CalculateDelta()
        {
            var length = end - start;
            deltaPerMs = length / (float)duration.TotalMilliseconds;
        }

        public void Tick(TimeSpan elapsed)
        {
            var animationTime = elapsed - loopOffset;
            var pos = start + (float)animationTime.TotalMilliseconds * deltaPerMs;
            if (animationTime >= duration)
            {
                pos = end;
                switch (loopMode)
                {
                    case LoopMode.None:
                        IsFinished = true;
                        break;
                    case LoopMode.Restart:
                        loopOffset = elapsed;
                        break;
                    case LoopMode.Reverse:
                        loopOffset = elapsed;
                        var temp = start;
                        start = end;
                        end = temp;
                        CalculateDelta();
                        break;
                }
            }
            target.SetValue(pos);
        }

        public void Start()
        {
            target.SetValue(start);
        }

        public bool IsFinished { get; private set; }
    }
}