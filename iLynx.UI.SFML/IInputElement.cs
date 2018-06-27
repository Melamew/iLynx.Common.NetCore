using SFML.System;

namespace iLynx.UI.Sfml
{
    public interface IInputElement : IRenderElement
    {
        /// <summary>
        /// Gets a value indicating whether or not this element is "visible" when hit testing.
        /// </summary>
        bool IsHitTestVisible { get; set; }

        /// <summary>
        /// Gets a value indicating whether or not this element is focusable
        /// </summary>
        bool IsFocusable { get; set; }

        /// <summary>
        /// Gets a value indicating whether or not this element has keyboard focus
        /// </summary>
        bool HasFocus { get; }

        /// <summary>
        /// Gets a value indicating whether or not the mouse cursor is over this element
        /// </summary>
        bool IsMouseOver { get; }

        /// <summary>
        /// Perform a "hit test" to check if the specified <see cref="Vector2f"/> (Position) is within the bounds of this element.
        /// </summary>
        /// <param name="position">The position (in global coordinates) to check against this element</param>
        /// <param name="element">The element that this position hit</param>
        /// <returns>True if the position is within this element, otherwise false</returns>
        bool HitTest(Vector2f position, out IInputElement element);
    }
}