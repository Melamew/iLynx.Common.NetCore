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
using System.Linq;
using OpenTK.Graphics.OpenGL;

namespace iLynx.UI.OpenGL.Shaders
{
    public class ShaderProgram
    {
        public ShaderProgram(int id)
        {
            Id = id;
        }

        public int Id { get; }

        public void UseProgram()
        {
            if (IsInUse) return;
            GL.UseProgram(Id);
            IsInUse = true;
        }

        //public void 

        public bool IsInUse { get; private set; }
    }
    public static class ShaderCompiler
    {
        public static int Compile(this IShader shader)
        {
            var type = shader.Type;
            var source = shader.Source;
            var glShader = GL.CreateShader(type);
            GL.ShaderSource(glShader, source);
            GL.CompileShader(glShader);
            return glShader;
        }

        public static int CompileProgram(params IShader[] shaders)
        {
            var program = GL.CreateProgram();
            var compiledShaders = shaders.Select(x => x.Compile()).ToArray();
            foreach (var shader in compiledShaders)
                GL.AttachShader(program, shader);
            GL.LinkProgram(program);
            GL.ValidateProgram(program);
            foreach (var shader in compiledShaders)
                GL.DeleteShader(shader);
            return 0;
        }
    }
}
