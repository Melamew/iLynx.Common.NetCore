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
using JetBrains.Annotations;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace iLynx.Graphics.Shaders
{
    public partial class Shader : IDisposable
    {
        protected const string DEFAULT_FRAGMENT_SHADER_REL_PATH = "Shaders/default.frag";
        protected const string DEFAULT_VERTEX_SHADER_REL_PATH = "Shaders/default.vert";

        /// <summary>
        /// The name of the view transform uniform variable in the vertex shader.
        /// This uniform must be declared in the shader source if <see cref="ViewTransform"/> and <see cref="SetTransform(Matrix4)"/> are in use
        /// </summary>
        public const string TRANSFORM_UNIFORM_NAME = "transform";

        private readonly int m_handle;
        private readonly bool m_isProgram;
        private readonly int m_transformId;

        /// <inheritdoc/>
        /// <remarks>
        /// <see cref="GL.DeleteProgram(int)"/>
        /// </remarks>
        public void Dispose()
        {
            GLCheck.Check(GL.DeleteProgram, m_handle);
        }

        /// <summary>
        /// Gets or Sets the view transform for this shader
        /// </summary>
        public Matrix4 ViewTransform { get; set; } = Matrix4.Identity;

        /// <summary>
        /// Creates a new shader program with the specified partial shaders
        /// </summary>
        /// <param name="shaders">The shaders to link as a full shader program</param>
        /// <remarks>
        /// <see cref="GL.LinkProgram(int)"/>
        /// <see cref="GL.GetUniformLocation(int,string)"/>
        /// </remarks>
        public Shader(params PartialShader[] shaders)
        {
            m_isProgram = true;
            try
            {
                m_handle = GLCheck.Check(GL.CreateProgram);
                foreach (var shader in shaders) shader.AttachToProgram(m_handle);
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
        /// Activates the specified shader
        /// </summary>
        /// <remarks>
        /// Calling this method with null is equivalent to unbinding the currently active shader program.
        /// <see cref="GL.UseProgram(int)"/>
        /// </remarks>
        public static void Activate([CanBeNull]Shader shader)
        {
            GLCheck.Check(GL.UseProgram, shader?.m_handle ?? 0);
        }
    }

    public partial class Shader
    {
        /// <summary>
        /// The default fragment shader included in <see cref="iLynx.Graphics"/>
        /// </summary>
        public static PartialShader DefaultFragmentShader { get; } = new Fragment(new FileInfo(DEFAULT_FRAGMENT_SHADER_REL_PATH));
        /// <summary>
        /// The default vertex shader included in <see cref="iLynx.Graphics"/>
        /// </summary>
        public static PartialShader DefaultVertexShader { get; } = new Vertex(new FileInfo(DEFAULT_VERTEX_SHADER_REL_PATH));
        public static Shader DefaultShader { get; } = new Shader(DefaultFragmentShader, DefaultVertexShader);

        /// <inheritdoc />
        /// <summary>
        /// Represents a fragment shader
        /// </summary>
        public class Fragment : PartialShader
        {
            /// <inheritdoc />
            /// <summary>
            /// Initializes a new fragment shader (<see cref="ShaderType.FragmentShader"/>) compiled from the specified source code
            /// </summary>
            /// <param name="shaderSource">The source code of the shader</param>
            public Fragment([NotNull]string shaderSource)
                : base(ShaderType.FragmentShader, shaderSource) { }

            /// <inheritdoc />
            /// <summary>
            /// Initializes a new fragment shader (<see cref="ShaderType.FragmentShader"/>) compiled from the source code contained in the specified file
            /// </summary>
            /// <param name="file">The file that contains the shader source code</param>
            public Fragment([NotNull]FileInfo file)
                : base(ShaderType.FragmentShader, file) { }
        }

        /// <inheritdoc />
        /// <summary>
        /// Represents a vertex shader
        /// </summary>
        public class Vertex : PartialShader
        {
            /// <inheritdoc />
            /// <summary>
            /// Initializes a new vertex shader (<see cref="ShaderType.VertexShader"/>) compiled from the specified source code
            /// </summary>
            /// <param name="shaderSource">The source code of the shader</param>
            public Vertex(string shaderSource)
                : base(ShaderType.VertexShader, shaderSource) { }

            /// <inheritdoc />
            /// <summary>
            /// Initializes a new vertex shader (<see cref="ShaderType.VertexShader"/>) compiled from the source code contained in the specified file
            /// </summary>
            /// <param name="file">The file that contains the shader source code</param>
            public Vertex(FileInfo file)
                : base(ShaderType.VertexShader, file) { }
        }

        /// <inheritdoc />
        /// <summary>
        /// Represents a generic GL shader object
        /// </summary>
        public abstract class PartialShader : IDisposable
        {
            private readonly int m_handle;

            /// <summary>
            /// Creates the handle of this shader part with the specified <see cref="ShaderType"/>
            /// </summary>
            /// <param name="type"></param>
            /// <remarks><see cref="GL.CreateShader"/></remarks>
            private PartialShader(ShaderType type)
            {
                Type = type;
                m_handle = GLCheck.Check(GL.CreateShader, type);
            }

            /// <inheritdoc />
            /// <summary>
            /// Loads the contents of the specified file as the source code for this shader and compiles it as the specified <see cref="ShaderType"/>.
            /// </summary>
            /// <param name="type">The type of shader to create</param>
            /// <param name="file">The file to load</param>
            protected PartialShader(ShaderType type, [NotNull]FileInfo file)
                : this(type)
            {
                if (!file.Exists || file.Length == 0) throw new FileNotFoundException();
                string shaderSource;
                using (var reader = file.OpenText())
                {
                    shaderSource = reader.ReadToEnd();
                }

                Initialize(shaderSource);
            }

            /// <inheritdoc />
            /// <summary>
            /// Initializes this shader with the specified <paramref name="source" /> string as source code and compiles it.
            /// </summary>
            /// <param name="type"></param>
            /// <param name="source"></param>
            protected PartialShader(ShaderType type, [NotNull]string source)
                : this(type)
            {
                Initialize(source);
            }

            private void Initialize(string shaderSource)
            {
                try
                {
                    GLCheck.Check(GL.ShaderSource, m_handle, shaderSource);
                    GLCheck.Check(GL.CompileShader, m_handle);
                }
                catch (OpenGLCallException callException)
                {
                    var error = GL.GetShaderInfoLog(m_handle);
                    throw new ShaderCompilationException(error, callException);
                }
            }

            /// <inheritdoc/>
            /// <remarks><see cref="GL.DeleteShader(int)"/></remarks>
            public void Dispose()
            {
                GLCheck.Check(GL.DeleteShader, m_handle);
            }

            /// <summary>
            /// Attaches this partial shader to the specified program (<paramref name="program"/>)
            /// </summary>
            /// <param name="program">This value should be a pointer to an opengl shader program object</param>
            public void AttachToProgram(int program)
            {
                GLCheck.Check(GL.AttachShader, program, m_handle);
            }

            /// <summary>
            /// Gets the <see cref="ShaderType"/> of this shader
            /// </summary>
            public ShaderType Type { get; }

            public static PartialShader FromFile(ShaderType type, [NotNull]string fileName)
            {
                return new GenericImpl(type, new FileInfo(fileName));
            }

            public static PartialShader FromSource(ShaderType type, [NotNull] string sourceCode)
            {
                return new GenericImpl(type, sourceCode);
            }

            private class GenericImpl : PartialShader
            {
                /// <inheritdoc />
                public GenericImpl(ShaderType type, [NotNull] FileInfo file)
                    : base(type, file)
                {
                }

                /// <inheritdoc />
                public GenericImpl(ShaderType type, [NotNull] string source)
                    : base(type, source)
                {
                }
            }
        }
    }
}
