#region LICENSE
/*
 * Copyright 2018 Melanie Hjorth
 *
 * Redistribution and use in source and binary forms,
 * with or without modification,
 * are permitted provided that the following conditions are met:
 *
 * 1. Redistributions of source code must retain the above copyright notice,
 * this list of conditions and the following disclaimer.
 *
 * 2. Redistributions in binary form must reproduce the above copyright notice,
 * this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
 *
 * 3. Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote
 * products derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
 * INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
 * HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
 * OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE,
 * EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 */
#endregion

using System;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace iLynx.Graphics.Rendering.Shaders
{
    public class Shader : IDisposable
    {
        private static Shader defaultFragmentShader;
        private static Shader default2DVertexShader;
        public const string DefaultFragmentShaderRelPath = "shaders/default.frag";
        public const string Default2DVertexShaderRelPath = "shaders/default2d.vert";

        public static Shader DefaultFragmentShader
        {
            get => defaultFragmentShader ?? (defaultFragmentShader = FromFile(ShaderType.FragmentShader, DefaultFragmentShaderRelPath));
        }

        public static Shader Default2DVertexShader
        {
            get => default2DVertexShader ?? (default2DVertexShader = FromFile(ShaderType.VertexShader, Default2DVertexShaderRelPath));
        }
        
        protected Shader(ShaderType type, string shaderSource)
        {
            Handle = GL.CreateShader(type);
            GL.ShaderSource(Handle, shaderSource);
            GL.CompileShader(Handle);
        }

        public int Handle { get; }

        public static Shader FromFile(ShaderType type, string fileName)
        {
            if (!File.Exists(fileName)) throw new FileNotFoundException();
            string source;
            using (var reader = new StreamReader(File.OpenRead(fileName)))
            {
                source = reader.ReadToEnd();
            }
            return new Shader(type, source);
        }

        public static Shader FromSource(ShaderType type, string source)
        {
            return new Shader(type, source);
        }

        public void Dispose()
        {
            GL.DeleteShader(Handle);
        }
    }
}