using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace iLynx.UI.OpenGL.Shaders
{
    public class ShaderService
    {
        private readonly Dictionary<string, int> shaders = new Dictionary<string, int>();

        public ShaderService() { }

        public int GetShader(string shaderId)
        {
            return shaders[shaderId];
        }

        public bool TryGetShader(string shaderId, out int shader)
        {
            return shaders.TryGetValue(shaderId, out shader);
        }

        public void AddShader(string shaderId, int shader)
        {

        }

        public void DeleteAll()
        {
            foreach (var shaderPair in shaders)
            {
                GL.DeleteProgram(shaderPair.Value);
            }
        }
    }
}
