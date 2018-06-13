using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace iLynx.UI.SFML.Controls
{
    public abstract class Geometry
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
    }
}