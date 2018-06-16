using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace iLynx.UI.Sfml.Rendering
{
    public abstract class Geometry : Drawable
    {
        private readonly List<Vertex> vertices = new List<Vertex>();

        public Vertex[] Vertices => vertices.ToArray();

        public PrimitiveType PrimitiveType { get; protected set; }

        protected virtual void AddVertex(Vector2f position, Color color)
        {
            AddVertex(new Vertex(position, color));
        }

        protected virtual void DeleteVertex(int index)
        {
            vertices.RemoveAt(index);
        }

        protected virtual void ClearVertices()
        {
            vertices.Clear();
        }

        protected virtual void AddVertex(params Vertex[] v)
        {
            vertices.AddRange(v);
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(Vertices, PrimitiveType, states);
        }
    }
}