using System;

namespace iLynx.UI.Sfml.Animation
{
    public interface IAnimation
    {
        void Tick(TimeSpan elapsed);
        
        bool IsFinished { get; }
    }
}