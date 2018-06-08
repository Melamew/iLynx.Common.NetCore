using System.Collections;
using System.Collections.Generic;
using iLynx.Common;
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

        public Transform Transform { get; protected set; }

        protected virtual void AddVertex(float x, float y)
        {
            AddVertex(new Vertex(new Vector2f(x, y)));
        }

        protected virtual void AddVertex(params Vertex[] v)
        {
            vertices.AddRange(v);
        }
    }

    public abstract class SfmlControlBase : BindingSource, Drawable
    {
        public static Transform GlobalTransform { get; private set; }

        static SfmlControlBase()
        {
            GlobalTransform = Transform.Identity;
        }

        public static void CombineGlobalTransform(Transform transform)
        {
            var local = GlobalTransform;
            local.Combine(transform);
            GlobalTransform = local;
        }

        public static void SetGlobalTransform(Transform transform)
        {
            GlobalTransform = transform;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            var geometry = GetGeometry();
            states.Shader = geometry.Shader ?? states.Shader;
            states.Transform.Combine(geometry.Transform);
            target.Draw(geometry.Vertices, geometry.PrimitiveType);
        }

        protected abstract Geometry GetGeometry();
    }
}
