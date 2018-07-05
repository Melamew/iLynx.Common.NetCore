using System;

namespace iLynx.Graphics
{
    public abstract class OpenGLException : Exception
    {
        protected OpenGLException() { }

        protected OpenGLException(string message) : base(message) { }

        protected OpenGLException(string message, Exception innerException) : base(message, innerException) { }
    }
}