using System;
using OpenTK.Graphics.OpenGL;

namespace iLynx.UI.OpenGL.Shaders
{
    public abstract class ShaderBase : IShader
    {
        private readonly ShaderType shaderType;
        private readonly string source;

        protected ShaderBase(ShaderType shaderType, string source)
        {
            this.shaderType = shaderType;
            this.source = source ?? throw new ArgumentNullException(nameof(source));
        }

        public virtual int Compile()
        {
            var glShader = GL.CreateShader(shaderType);
            GL.ShaderSource(glShader, source);
            GL.CompileShader(glShader);
            return glShader;
        }
    }
}