using System;
using System.ComponentModel;
using SFML.Graphics;
using SFML.System;

namespace iLynx.UI.Sfml
{
    // ReSharper disable once InconsistentNaming
    public interface IUIElement : Drawable
    {
        /// <summary>
        /// Lays out this element within the specified <see cref="FloatRect"/>
        /// </summary>
        /// <param name="target"></param>
        FloatRect Layout(FloatRect target);

        /// <summary>
        /// Given the available space (<paramref name="available"/>), returns the space taken up by this element.
        /// </summary>
        /// <param name="available"></param>
        /// <returns></returns>
        Vector2f Measure(Vector2f available);

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
        /// Gets the bounding box of this element
        /// </summary>
        FloatRect BoundingBox { get; }
    }
}
