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
            return (BoundingBox = ComputeBoundingBox(target)) + margin;
        }

        protected virtual Transform ComputeRenderTransform(FloatRect destinationRect)
        {
            var transform = Transform.Identity;
            ComputedPosition = destinationRect.Position();
            transform.Translate(ComputedPosition);
            return transform;
        }

        protected abstract FloatRect ComputeBoundingBox(FloatRect destinationRect);

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

        public event EventHandler<PropertyChangedEventArgs> LayoutPropertyChanged;

        public Vector2f ComputedPosition { get; private set; } = new Vector2f(0f, 0f);

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