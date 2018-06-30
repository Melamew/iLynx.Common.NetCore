using System;

namespace iLynx.Graphics.Rendering
{
    public interface IRenderContext
    {
        /// <summary>
        /// Binds the specified <see cref="VertexBuffer{TVertex}"/> to 
        /// </summary>
        /// <typeparam name="TVertex"></typeparam>
        /// <param name="buffer"></param>
        void Bind<TVertex>(VertexBuffer<TVertex> buffer) where TVertex : struct, IEquatable<TVertex>;

        /// <summary>
        /// Sets the specified <see cref="ShaderProgram"/> as the current shader
        /// </summary>
        /// <param name="program"></param>
        void UseShader(ShaderProgram program);

        /// <summary>
        /// Binds the specified <see cref="Texture"/> for use in the current <see cref="IRenderContext"/>
        /// </summary>
        /// <param name="texture"></param>
        void BindTexture(Texture texture);

        /// <summary>
        /// Draws the currently bound <see cref="VertexBuffer{TVertex}"/>
        /// </summary>
        void Draw();

        /// <summary>
        /// Binds the specified <see cref="VertexBuffer{TVertex}"/> and draws it with the currently set texture and shader
        /// </summary>
        /// <typeparam name="TVertex"></typeparam>
        /// <param name="vertexBuffer"></param>
        void Draw<TVertex>(VertexBuffer<TVertex> vertexBuffer) where TVertex : struct, IEquatable<TVertex>;
    }
}
