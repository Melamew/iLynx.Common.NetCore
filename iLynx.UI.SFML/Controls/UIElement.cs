using iLynx.Common;
using SFML.Graphics;

namespace iLynx.UI.SFML.Controls
{
    public abstract class UIElement : BindingSource, Drawable
    {
        public abstract void Draw(RenderTarget target, RenderStates states);
    }
}