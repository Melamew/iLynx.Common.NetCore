using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using iLynx.Common;
using iLynx.UI.Sfml;
using SFML.Graphics;
using SFML.System;

namespace iLynx.UI.SFML.Controls
{
    // ReSharper disable once InconsistentNaming
    public abstract partial class UIElement : BindingSource, IUIElement
    {
        private Transform savedTransform = Transform.Identity;
        private Thickness margin = Thickness.Zero;
        protected Transform RenderTransform = Transform.Identity;
        private Vector2f computedPosition = new Vector2f(0f, 0f);

        public void Draw(RenderTarget target, RenderStates states)
        {
            ApplyTransform(RenderTransform, ref states);
            DrawTransformed(target, states);
            ResetTransform(ref states);
        }

        protected void ApplyTransform(Transform t, ref RenderStates states)
        {
            savedTransform = states.Transform;
            var transform = savedTransform;
            transform.Combine(t);
            states.Transform = transform;
        }

        protected void ResetTransform(ref RenderStates states)
        {
            states.Transform = savedTransform;
        }

        public virtual FloatRect Layout(FloatRect target)
        {
            target -= margin;
            RenderTransform = ComputeRenderTransform(target);
            return BoundingBox = ComputeBoundingBox(target);
        }

        protected virtual Transform ComputeRenderTransform(FloatRect destinationRect)
        {
            var transform = Transform.Identity;
            computedPosition = destinationRect.Position();
            transform.Translate(computedPosition);
            return transform;
        }

        protected abstract FloatRect ComputeBoundingBox(FloatRect destinationRect);
        //{
        //    return destinationRect;
        //}

        public Thickness Margin
        {
            get => margin;
            set
            {
                if (value == margin) return;
                var old = margin;
                margin = value;
                OnPropertyChanged(old, margin);
                OnLayoutPropertyChanged();
            }
        }

        protected virtual void OnLayoutPropertyChanged([CallerMemberName] string propertyName = null)
        {
            LayoutPropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnRenderPropertyChanged([CallerMemberName] string propertyName = null)
        {
            RenderPropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event EventHandler<PropertyChangedEventArgs> LayoutPropertyChanged;
        public event EventHandler<PropertyChangedEventArgs> RenderPropertyChanged;

        public Vector2f ComputedPosition => computedPosition;

        //public Vector2f

        public FloatRect BoundingBox { get; private set; }
    }

    // ReSharper disable once InconsistentNaming
    public abstract partial class UIElement : BindingSource, IUIElement
    {
        static UIElement()
        {
            DefaultFont = new Font("fonts/Mechanical.otf");
        }
        public static Font DefaultFont { get; }
        protected abstract void DrawTransformed(RenderTarget target, RenderStates states);
    }
}