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

namespace iLynx.Graphics.shaders
{
    public class ShaderCompilationException : OpenGLException
    {
        public ShaderCompilationException(string errorMessage, Exception innerException)
            : base($"Shader compilation failed with error: {errorMessage}", innerException) { }
    }

    public class Shader : IDisposable
    {
        private static Shader defaultFragmentShader;
        private static Shader defaultVertexShader;
        private readonly int handle;
        public const string DefaultFragmentShaderRelPath = "shaders/default.frag";
        public const string DefaultVertexShaderRelPath = "shaders/default.vert";

        public static Shader DefaultFragmentShader => defaultFragmentShader ?? (defaultFragmentShader = FromFile(ShaderType.FragmentShader, DefaultFragmentShaderRelPath));

        public static Shader DefaultVertexShader => defaultVertexShader ?? (defaultVertexShader = FromFile(ShaderType.VertexShader, DefaultVertexShaderRelPath));

        protected Shader(ShaderType type, string shaderSource)
        {
            handle = GL.CreateShader(type);
            try
            {
                GLCheck.Check(GL.ShaderSource, handle, shaderSource);
                GLCheck.Check(GL.CompileShader, handle);
            }
            catch (OpenGLCallException callException)
            {
                var error = GL.GetShaderInfoLog(handle);
                throw new ShaderCompilationException(error, callException);
            }
        }

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
            GL.DeleteShader(handle);
        }

        public void AttachToProgram(int program)
        {
            GLCheck.Check(GL.AttachShader, program, handle);
        }
    }
}