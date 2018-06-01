using System.Collections;
using System.Collections.Generic;
using iLynx.Common;
using SFML.Graphics;

namespace iLynx.UI.SFML.Controls
{
    public abstract class Geometry
    {
        private readonly List<Vertex> vertices = new List<Vertex>();
        public Vertex[] Vertices => vertices.ToArray();

        public PrimitiveType PrimitiveType { get; protected set; }
    }

    public abstract class SfmlControlBase : BindingSource, Drawable
    {
        public void Draw(RenderTarget target, RenderStates states)
        {
            throw new System.NotImplementedException();
        }
    }
}
