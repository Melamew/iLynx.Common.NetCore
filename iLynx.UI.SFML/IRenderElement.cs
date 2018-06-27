using iLynx.Common;
using SFML.Graphics;
using SFML.System;

namespace iLynx.UI.Sfml
{
    public interface IRenderElement : Drawable, IBindingSource
    {
        /// <summary>
        /// Gets the bounding box of this element
        /// </summary>
        FloatRect BoundingBox { get; }

        /// <summary>
        /// Gets the parent element (if any) of this element.
        /// If this element does not have a parent (IE. It is the root of a tree) this property will return null
        /// </summary>
        IRenderElement Parent { get; }

        /// <summary>
        /// Called when this element is added to or removed from a content control, panel or other elements with children
        /// WARNING: Do not call this method unless the parent of this element has ACTUALLY changed.
        /// </summary>
        /// <param name="parent"></param>
        void SetLogicalParent(IRenderElement parent);

        /// <summary>
        /// Transforms the specified coordinates from global to local coordinates (coordinates relative to this element)
        /// </summary>
        /// <param name="coords"></param>
        /// <returns></returns>
        Vector2f ToLocalCoords(Vector2f coords);

        /// <summary>
        /// Transforms the specified coordinates from local to global coordinates (coordinates relative to this element)
        /// </summary>
        /// <param name="coords"></param>
        /// <returns></returns>
        Vector2f ToGlobalCoords(Vector2f coords);

        /// <summary>
        /// Gets the size of this element in "render coordinates"
        /// </summary>
        Vector2f RenderSize { get; }

        /// <summary>
        /// Raised when the bounding box of this element has changed
        /// </summary>
        event ValueChangedEventHandler<IRenderElement, FloatRect> BoundingBoxChanged;

        /// <summary>
        /// Raised when the render size of this element has changed
        /// </summary>
        event ValueChangedEventHandler<IRenderElement, Vector2f> RenderSizeChanged;

        /// <summary>
        /// Raised when the render position of this element has changed
        /// </summary>
        event ValueChangedEventHandler<IRenderElement, Vector2f> RenderPositionChanged;
    }
}