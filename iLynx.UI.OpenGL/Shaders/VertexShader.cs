using OpenTK.Graphics.OpenGL;

namespace iLynx.UI.OpenGL.Shaders
{
    public class VertexShader : ShaderBase
    {
        public VertexShader(string shaderSource) : base(ShaderType.VertexShader, shaderSource) { }
    }
}
