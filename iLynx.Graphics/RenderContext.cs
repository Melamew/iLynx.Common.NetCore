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
using iLynx.Graphics.Shaders;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;

namespace iLynx.Graphics
{
    public class RenderContext : IRenderContext
    {
        private Shader activeShader;
        private Texture activeTexture;
        private readonly IWindowInfo window;
        private readonly IGraphicsContext graphicsContext;
        private Matrix4 viewTransform = Matrix4.Identity;

        public RenderContext(IGraphicsContext graphicsContext, IWindowInfo window)
        {
            this.graphicsContext = graphicsContext;
            this.window = window;
        }

        public bool IsCurrent => graphicsContext.IsCurrent;

        public bool MakeCurrent()
        {
            try
            {
                graphicsContext.MakeCurrent(window);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to make current context active: {e}");
                return false;
            }
        }

        public bool IsInitialized { get; private set; }

        public void Initialize()
        {
            if (IsInitialized) return;
            if (!graphicsContext.IsCurrent)
                graphicsContext.MakeCurrent(window);
            GLCheck.Check(GL.Enable, EnableCap.Blend);
            GLCheck.Check(GL.BlendFunc, BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GLCheck.Check(GL.Enable, EnableCap.CullFace);
            GLCheck.Check(GL.CullFace, CullFaceMode.Back);
            GLCheck.Check(GL.Enable, EnableCap.DepthTest);
            GLCheck.Check(GL.DepthFunc, DepthFunction.Less);
            IsInitialized = true;
        }

        public Shader Shader
        {
            get => activeShader;
            set
            {
                if (value == activeShader) return;
                EnsureActive();
                activeShader = value;
                value.ViewTransform = viewTransform;
                value.Activate();
            }
        }

        public Texture Texture
        {
            get => activeTexture;
            set
            {
                if (value == activeTexture) return;
                EnsureActive();
                activeTexture = value;
                value.Bind();
            }
        }
        public Matrix4 ViewTransform
        {
            get => viewTransform;
            set
            {
                if (value == viewTransform) return;
                EnsureActive();
                viewTransform = value;
                if (null == activeShader) return;
                activeShader.ViewTransform = value;
            }
        }

        public void ApplyTransform(Matrix4 transform)
        {
            activeShader?.SetTransform(transform);
        }

        private void EnsureActive()
        {
            if (!IsCurrent && !MakeCurrent())
                throw new InvalidOperationException("Unable to activate context for shader activation");
        }
    }
}