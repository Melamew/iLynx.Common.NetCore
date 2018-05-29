using OpenTK.Graphics.OpenGL;

namespace iLynx.UI.OpenGL.Shaders
{
    public class FragmentShader : ShaderBase
    {
        public FragmentShader(string shaderSource) : base(ShaderType.FragmentShader, shaderSource) { }
    }
}