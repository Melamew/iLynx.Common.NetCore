using SFML.Graphics;
using SFML.System;

namespace iLynx.UI.SFML
{
    // ReSharper disable once InconsistentNaming
    public interface IUIElement : Drawable
    {
        /// <summary>
        /// Gets a <see cref="IntRect"/> that defines the "bounding box" of this element (The rectangle that bounds the pixels that this element displays).
        /// </summary>
        FloatRect BoundingBox { get; }
    }
}
