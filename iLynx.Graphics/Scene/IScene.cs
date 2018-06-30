using System;
using System.Collections.Generic;
using iLynx.Graphics.Rendering;

namespace iLynx.Graphics.Scene
{
    public interface IScene : IDisposable
    {
        void Display(IRenderContext context);

        void Update();

        ISceneObject Root { get; set; }
    }
}
