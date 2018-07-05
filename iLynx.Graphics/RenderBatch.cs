using System;
using System.Collections.Generic;
using iLynx.Graphics.Shaders;
using OpenTK;

namespace iLynx.Graphics
{
    public class RenderBatch
    {
        private readonly List<DrawCall<Vertex>> drawCalls = new List<DrawCall<Vertex>>();
        public Texture Texture { get; set; }

        public Shader Shader { get; set; }

        public void AddCall(DrawCall<Vertex> call)
        {
            drawCalls.Add(call);
        }

        public void RermoveCall(DrawCall<Vertex> call)
        {
            drawCalls.Remove(call);
        }

        public void Execute(/*Matrix4 viewTransform*/)
        {
            if (null == Shader) throw new InvalidOperationException("Cannot draw anything without a shader");
            //Shader.ViewTransform = viewTransform;
            //Shader.Activate();
            foreach (var call in drawCalls)
                call.Execute(Shader);
        }
    }
}