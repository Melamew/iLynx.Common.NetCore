namespace iLynx.Graphics
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