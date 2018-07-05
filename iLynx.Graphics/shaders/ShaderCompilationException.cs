using System;

namespace iLynx.Graphics.Shaders
{
    public class ShaderCompilationException : OpenGLException
    {
        public ShaderCompilationException(string errorMessage, Exception innerException)
            : base($"Shader compilation failed with error: {errorMessage}", innerException) { }
    }
}