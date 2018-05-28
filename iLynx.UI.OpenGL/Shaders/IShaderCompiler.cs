using OpenTK.Graphics.OpenGL;

namespace iLynx.UI.OpenGL.Shaders
{
    internal interface IShaderCompiler<in TRenderable> where TRenderable : IRenderable
    {
        /// <summary>
        /// Compiles shaders neccessary for rendering the specified element
        /// </summary>
        /// <param name="element">The <see cref="IRenderable"/> to generate shaders for</param>
        /// <returns></returns>
        int Compile(TRenderable element);
    }

    //public class UIShaderCompiler : IShaderCompiler<IUIElement>
    //{
    //    public int Compile(IUIElement element)
    //    {
    //        GL.GenBuffers(1, out int vertexBuffer);
    //        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            
    //        //GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, )
    //        //var vertexShader = GL.CreateShader(ShaderType.VertexShader);
    //        //GL.vertex

    //        return 0;
    //    }
    //}
}
