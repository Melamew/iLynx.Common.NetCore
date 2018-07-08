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
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace iLynx.Graphics.Shaders
{
    public class Shader : IDisposable
    {
        private readonly int m_handle;
        private readonly bool m_isProgram;
        protected const string DEFAULT_FRAGMENT_SHADER_REL_PATH = "Shaders/default.frag";
        protected const string DEFAULT_VERTEX_SHADER_REL_PATH = "Shaders/default.vert";
        public const string TRANSFORM_UNIFORM_NAME = "transform";
        private readonly int m_transformId;

        public static Shader DefaultFragmentShader { get; } = FromFile(ShaderType.FragmentShader, DEFAULT_FRAGMENT_SHADER_REL_PATH);

        public static Shader DefaultVertexShader { get; } = FromFile(ShaderType.VertexShader, DEFAULT_VERTEX_SHADER_REL_PATH);

        public static Shader DefaultShader { get; } = new Shader(DefaultFragmentShader, DefaultVertexShader);

        private Shader(ShaderType type, string shaderSource)
        {
            try
            {
                m_handle = GLCheck.Check(GL.CreateShader, type);
                GLCheck.Check(GL.ShaderSource, m_handle, shaderSource);
                GLCheck.Check(GL.CompileShader, m_handle);
            }
            catch (OpenGLCallException callException)
            {
                var error = GL.GetShaderInfoLog(m_handle);
                throw new ShaderCompilationException(error, callException);
            }
        }

        /// <summary>
        /// Loads the contents of the specified file and creates a shader of the specified type
        /// </summary>
        /// <param name="type">The <see cref="ShaderType"/> type of the shader</param>
        /// <param name="fileName">The path to the file containing the source code of the shader</param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates a shader of the specified type from the specified source
        /// </summary>
        /// <param name="type">The <see cref="ShaderType"/> type of the shader</param>
        /// <param name="source">The source code for the shader</param>
        /// <returns></returns>
        public static Shader FromSource(ShaderType type, string source)
        {
            return new Shader(type, source);
        }

        public void Dispose()
        {
            if (m_isProgram)
                GL.DeleteProgram(m_handle);
            else
                GL.DeleteShader(m_handle);
        }

        private void AttachToProgram(int program)
        {
            GLCheck.Check(GL.AttachShader, program, m_handle);
        }

        /// <summary>
        /// Gets or Sets the view transform for this shader
        /// </summary>
        public Matrix4 ViewTransform { get; set; } = Matrix4.Identity;

        /// <summary>
        /// Creates a new shader program with the specified shaders
        /// (GL.LinkProgram)
        /// </summary>
        /// <param name="shaders"></param>
        public Shader(params Shader[] shaders)
        {
            m_isProgram = true;
            try
            {
                m_handle = GLCheck.Check(GL.CreateProgram);
                foreach (var shader in shaders)
                    shader.AttachToProgram(m_handle);
                GLCheck.Check(GL.LinkProgram, m_handle);
                m_transformId = GLCheck.Check(GL.GetUniformLocation, m_handle, TRANSFORM_UNIFORM_NAME);
            }
            catch (OpenGLCallException e)
            {
                throw new ShaderCompilationException("", e);
            }
        }

        /// <summary>
        /// Sets the transform on the GPU for this shader (The final transform set is: transform * ViewTransform)
        /// (GL.UniformMatrix4)
        /// </summary>
        /// <param name="transform"></param>
        public void SetTransform(Matrix4 transform)
        {
            if (!m_isProgram) throw new InvalidOperationException("This shader has not been linked to a program");
            if (0 == m_handle) throw new NotInitializedException();
            if (m_transformId == -1) throw new InvalidOperationException("The specified shader does not have a transform input (uniform)");
            transform = transform * ViewTransform;
            GL.UniformMatrix4(m_transformId, false, ref transform);
        }

        /// <summary>
        /// Activates this shader (GL.UseProgram)
        /// </summary>
        public void Activate()
        {
            GLCheck.Check(GL.UseProgram, m_handle);
        }
    }
}