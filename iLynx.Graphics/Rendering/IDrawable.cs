using System;
using System.Collections.Generic;
using System.Text;

namespace iLynx.Graphics.Rendering
{
    /// <summary>
    /// Base interface for drawable elements.
    /// NOTE: A drawable must NOT call <see cref="IRenderTarget.DrawArrays"/>
    /// </summary>
    public interface IDrawable
    {
        /// <summary>
        /// Draws this <see cref="IDrawable"/> to the specified <see cref="IRenderTarget"/>
        /// </summary>
        /// <param name="target"></param>
        void Draw(IRenderTarget target);
    }
}
