using System;
using OpenTK.Graphics.OpenGL;

namespace iLynx.Graphics
{
    public class OpenGLCallException : OpenGLException
    {
        public OpenGLCallException(Delegate action, ErrorCode error)
            : base($"{action.Target}.{action.Method} failed with error: {error}")
        {
        }
    }
}