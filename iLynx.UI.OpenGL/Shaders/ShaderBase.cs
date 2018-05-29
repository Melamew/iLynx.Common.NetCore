using System;
using OpenTK.Graphics.OpenGL;

namespace iLynx.UI.OpenGL.Shaders
{
    public abstract class ShaderBase : IShader
    {
        protected ShaderType ShaderType;
        protected string ShaderSource;

        protected ShaderBase(ShaderType shaderType, string shaderSource)
        {
            ShaderType = shaderType;
            ShaderSource = shaderSource ?? throw new ArgumentNullException(nameof(shaderSource));
        }

        public string Source => ShaderSource;

        public ShaderType Type => ShaderType;
    }
}