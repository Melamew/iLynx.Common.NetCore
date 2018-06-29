namespace iLynx.UI.OpenGL.Rendering
{
    public interface IDrawable
    {
        /// <summary>
        /// Draws this element on to the specified <see cref="IRenderTarget"/>
        /// </summary>
        /// <param name="target"></param>
        void Draw(IRenderTarget target);
    }
}