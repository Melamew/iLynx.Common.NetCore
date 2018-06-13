using System;
using static System.MathF;
using System.Linq;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace iLynx.UI.SFML.Controls
{
    // ReSharper disable once InconsistentNaming
    public abstract class SfmlControlBase : UIElement, IControl
    {
        //private volatile bool isDirty = true;
        //private Geometry cachedGeometry;

        //protected override void DrawTransformed(RenderTarget target, RenderStates states)
        //{
        //    //var geometry = GetGeometry();
        //    if (null == geometry) return;
        //    target.Draw(geometry.Vertices, geometry.PrimitiveType, states);
        //}

        ////private Geometry GetGeometry()
        ////{
        ////    try
        ////    {
        ////        return cachedGeometry = isDirty ? GenerateGeometry() : cachedGeometry;
        ////    }
        ////    finally { isDirty = false; }
        ////}

        //protected abstract Geometry GenerateGeometry();

        //protected virtual void SetDirty()
        //{
        //    isDirty = true;
        //}
        public event EventHandler<MouseButtonEventArgs> Clicked;
    }
}
