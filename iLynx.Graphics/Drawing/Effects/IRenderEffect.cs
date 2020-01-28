namespace iLynx.Graphics.Drawing.Effects
{
    public interface IRenderEffect
    {
        /// <summary>
        /// Gets the shader that this effect will use when applied to an <see cref="IDrawable"/>
        /// </summary>
        Shader Shader { get; }
    }

    public class OutlineEffect : IRenderEffect
    {
        private readonly float m_thickness;
        public Shader Shader { get; }

        public OutlineEffect(float thickness)
        {
            m_thickness = thickness;
        }
    }
}
