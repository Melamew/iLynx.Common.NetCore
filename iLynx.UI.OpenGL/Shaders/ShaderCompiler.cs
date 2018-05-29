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
