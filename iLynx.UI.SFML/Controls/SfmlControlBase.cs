using System;
using static System.MathF;
using System.Collections.Generic;
using System.Linq;
using iLynx.Common;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

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

    // ReSharper disable once InconsistentNaming
    public abstract class UIElement : BindingSource, Drawable
    {
        public abstract void Draw(RenderTarget target, RenderStates states);
    }

    public abstract class SfmlControlBase : UIElement, IControl
    {
        public static Transform GlobalTransform { get; private set; }

        static SfmlControlBase()
        {
            GlobalTransform = Transform.Identity; // 3x3 identity matrix
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

        public override void Draw(RenderTarget target, RenderStates states)
        {
            var geometry = Geometry;
            if (null == geometry) return;
            states.Shader = geometry.Shader ?? states.Shader;
            var transform = states.Transform;
            transform.Combine(geometry.Transform);
            //states.Transform.Combine(geometry.Transform);
            target.Draw(geometry.Vertices, geometry.PrimitiveType);
        }

        protected abstract Geometry Geometry { get; }


        //TODO: Optimize later
        public IntRect BoundingBox
        {
            get
            {
                var geometry = Geometry;
                var transform = GlobalTransform;
                transform.Combine(Geometry.Transform);
                float minX = float.MaxValue, minY = float.MaxValue, maxX = float.MinValue, maxY = float.MinValue;
                foreach (var vector in geometry.Vertices.Select(x => transform.TransformPoint(x.Position)))
                {
                    minX = Min(vector.X, minX);
                    minY = Min(vector.Y, minY);
                    maxX = Max(vector.X, maxX);
                    maxY = Max(vector.Y, maxY);
                }
                return new IntRect((int)Round(minX), (int)Round(minY), (int)Round(maxX - minX), (int)Round(maxY - minY));
            }
        }

        public event EventHandler<MouseButtonEventArgs> Clicked;
    }
}
