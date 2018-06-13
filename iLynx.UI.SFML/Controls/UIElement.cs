using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using iLynx.Common;
using SFML.Graphics;

namespace iLynx.UI.SFML.Controls
{
    // ReSharper disable once InconsistentNaming
    public abstract partial class UIElement : BindingSource, IUIElement
    {
        private Transform savedTransform;
        private Thickness margin;
        protected Transform RenderTransform;
        private FloatRect boundingBox;
        //private Vector2f position;

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

        public void Layout(FloatRect target)
        {
            PrepareRender();
            RenderTransform = ComputeRenderTransform(boundingBox);
            boundingBox = ComputeBoundingBox(target);
        }

        protected virtual Transform ComputeRenderTransform(FloatRect destinationRect)
        {
            var transform = Transform.Identity;
            transform.Translate(destinationRect.Left, destinationRect.Top);
            return transform;
        }

        protected virtual FloatRect ComputeBoundingBox(FloatRect destinationRect)
        {
            if (Thickness.IsNaN(margin)) return new FloatRect();
            return new FloatRect(
                destinationRect.Left + margin.Left,
                destinationRect.Top + margin.Top,
                destinationRect.Width - margin.Horizontal,
                destinationRect.Height - margin.Vertical);
        }

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
            PrepareRender();
            RenderPropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event EventHandler<PropertyChangedEventArgs> LayoutPropertyChanged;
        public event EventHandler<PropertyChangedEventArgs> RenderPropertyChanged;

        /// <summary>
        /// Called whenever there is an update to the render properties of this element, or the layout has been computed.
        /// </summary>
        protected abstract void PrepareRender();

        ///// <summary>
        ///// Gets or Sets the position of this element; note that setting this value will overwrite the <see cref="Margin"/>
        ///// </summary>
        //public Vector2f Position
        //{
        //    get => position;
        //    set
        //    {
        //        if (value == position) return;
        //        var old = position;
        //        margin = Thickness.NaN;
        //        position = value;
        //        OnPropertyChanged(old, value);
        //    }
        //}

        public FloatRect BoundingBox => boundingBox;
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