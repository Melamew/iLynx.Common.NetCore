using System;

namespace iLynx.UI.Sfml.Animation
{
    public interface IAnimation
    {
        void Tick(TimeSpan elapsed);

        void Start();

        bool IsFinished { get; }
    }
}