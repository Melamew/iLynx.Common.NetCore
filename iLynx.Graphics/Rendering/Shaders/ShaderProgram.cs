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
using OpenTK;
using OpenTK.Graphics.OpenGL;
using static iLynx.Graphics.GLCheck;

namespace iLynx.Graphics.Rendering.Shaders
{
    public class ShaderProgram : IDisposable
    {
        public const string TransformUniformName = "transform";
        private static ShaderProgram default2DShader;

        public static ShaderProgram Default2DShader => default2DShader ?? (default2DShader =
                                                           new ShaderProgram(Shader.DefaultFragmentShader, Shader.Default2DVertexShader));

        private readonly int handle;

        public Matrix4 ViewTransform { get; set; } = Matrix4.Identity;

        public ShaderProgram(params Shader[] shaders)
        {
            try
            {
                handle = Check(GL.CreateProgram);
                foreach (var shader in shaders)
                    shader.AttachToProgram(handle);
                Check(GL.LinkProgram, handle);
            }
            catch (OpenGLCallException e)
            {
                throw new ShaderCompilationException("", e);
            }
        }

        public void SetTransform(Matrix4 transform)
        {
            int location;
            if (0 == handle) throw new NotInitializedException();
            if ((location = GL.GetUniformLocation(handle, TransformUniformName)) == -1) return;
            transform *= ViewTransform;
            GL.UniformMatrix4(location, false, ref transform);
        }

        public static void Use(ShaderProgram program)
        {
            GL.UseProgram(program?.handle ?? 0);
        }

        public void Dispose()
        {
            GL.DeleteProgram(handle);
        }
    }
}