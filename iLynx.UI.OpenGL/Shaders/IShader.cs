using OpenTK.Graphics.OpenGL;

namespace iLynx.UI.OpenGL.Shaders
{
    public interface IShader
    {
        /// <summary>
        /// The source code of the shader
        /// //TODO: Is it possible to use any other kind of shader source? pre-compile some bits, anything?
        /// //TODO: Keep an eye on ShaderGen (https://github.com/mellinoe/ShaderGen)
        /// </summary>
        string Source { get; }

        /// <summary>
        /// The <see cref="ShaderType"/> of this shader
        /// </summary>
        ShaderType Type { get; }
    }
}