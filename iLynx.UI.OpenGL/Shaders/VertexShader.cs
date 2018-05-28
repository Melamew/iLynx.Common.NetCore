using OpenTK.Graphics.OpenGL;

namespace iLynx.UI.OpenGL.Shaders
{
    public class VertexShader : ShaderBase
    {
        public VertexShader(string source) : base(ShaderType.VertexShader, source) { }
    }
}
