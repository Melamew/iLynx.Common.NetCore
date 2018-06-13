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

        public Shader Shader { get; protected set; }

        public Texture Texture { get; protected set; }

        public Transform Transform { get; protected set; } = Transform.Identity;

        protected virtual void AddVertex(float x, float y)
        {
            AddVertex(new Vertex(new Vector2f(x, y)));
        }

        protected virtual void AddVertex(params Vertex[] v)
        {
            vertices.AddRange(v);
        }
    }
}