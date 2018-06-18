using System;

namespace iLynx.UI.Sfml.Animation
{
    public class CallbackAnimation : Animation
    {
        private readonly Action<double> animationTick;

        public CallbackAnimation(Action<double> animationTick, TimeSpan duration, LoopMode loopMode = LoopMode.None, Func<double, double> easingFunction = null)
            : base(duration, loopMode, easingFunction)
        {
            this.animationTick = animationTick ?? throw new ArgumentNullException(nameof(animationTick));
        }

        protected override void Tick(double timeIndex)
        {
            animationTick(timeIndex);
        }
    }
}