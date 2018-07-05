using System;
using iLynx.Graphics.Shaders;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace iLynx.Graphics
{
    public class DrawCall<TVertex> where TVertex : struct, IVAOElement, IEquatable<TVertex>
    {
        private readonly Matrix4 transform;
        private readonly VertexArrayObject<TVertex> vertexArrayObject;
        private readonly int vertexCount;
        private readonly PrimitiveType primitiveType;

        public DrawCall(Matrix4 transform, PrimitiveType primitiveType, VertexArrayObject<TVertex> vertexArrayObject,
            int vertexCount)
        {
            this.transform = transform;
            this.primitiveType = primitiveType;
            this.vertexArrayObject = vertexArrayObject;
            this.vertexCount = vertexCount;
        }

        public void Execute(Shader shader)
        {
            vertexArrayObject.Bind();
            shader.SetTransform(transform);
            GL.DrawElements(primitiveType, vertexCount, DrawElementsType.UnsignedInt, IntPtr.Zero);
            vertexArrayObject.Unbind();
        }
    }
}