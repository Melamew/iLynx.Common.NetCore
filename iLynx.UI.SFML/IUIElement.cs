using System;
using System.ComponentModel;
using SFML.Graphics;
using SFML.System;

namespace iLynx.UI.SFML
{
    // ReSharper disable once InconsistentNaming
    public interface IUIElement : Drawable
    {
        /// <summary>
        /// Lays out this element within the specified <see cref="FloatRect"/>
        /// </summary>
        /// <param name="target"></param>
        void Layout(FloatRect target);

        /// <summary>
        /// Gets or Sets the margin of this element
        /// </summary>
        Thickness Margin { get; set; }

        /// <summary>
        /// Fired whenever a property that would affect the layout of this element is changed.
        /// (ie: <see cref="Margin"/>)
        /// </summary>
        event EventHandler<PropertyChangedEventArgs> LayoutPropertyChanged;

        /// <summary>
        /// Fired whenever a property that would affect the rendered result of this element is changed.
        /// (ie: Background Color)
        /// </summary>
        event EventHandler<PropertyChangedEventArgs> RenderPropertyChanged;

        FloatRect BoundingBox { get; }
    }
}
