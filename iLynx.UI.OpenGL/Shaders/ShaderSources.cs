using System;
using System.Collections.Generic;
using System.Text;

namespace iLynx.UI.OpenGL.Shaders
{
    public static class ShaderSources
    {
        public const string DEFAULT_VERTEX_SHADER = @"
#version 440
layout(location = 0) in vec4 position;

void main()
{
    gl_Position = position;
}
";

        public const string DEFAULT_FRAGMENT_SHADER = @"
#version 440
layout(location = 0) out vec4 color;

void main()
{
    color = vec4(0.2, 0.3, 0.8);
}
";
    }
}
