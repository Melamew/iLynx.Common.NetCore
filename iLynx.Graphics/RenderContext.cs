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
        private Shader m_activeShader;
        private Texture m_activeTexture;
        private readonly IWindowInfo m_window;
        private readonly IGraphicsContext m_graphicsContext;
        private Matrix4 m_viewTransform = Matrix4.Identity;

        public RenderContext(IGraphicsContext graphicsContext, IWindowInfo window)
        {
            m_graphicsContext = graphicsContext;
            m_window = window;
        }

        public bool IsCurrent => m_graphicsContext.IsCurrent;

        public bool MakeCurrent()
        {
            try
            {
                m_graphicsContext.MakeCurrent(m_window);
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
            if (!m_graphicsContext.IsCurrent)
                m_graphicsContext.MakeCurrent(m_window);
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
            get => m_activeShader;
            set
            {
                if (value == m_activeShader) return;
                EnsureActive();
                m_activeShader = value;
                if (null != value)
                    value.ViewTransform = m_viewTransform;
                Shader.Activate(value);
            }
        }

        public Texture Texture
        {
            get => m_activeTexture;
            set
            {
                if (value == m_activeTexture) return;
                EnsureActive();
                m_activeTexture = value;
                Texture.Bind(value);
            }
        }
        public Matrix4 ViewTransform
        {
            get => m_viewTransform;
            set
            {
                if (value == m_viewTransform) return;
                EnsureActive();
                m_viewTransform = value;
                if (null == m_activeShader) return;
                m_activeShader.ViewTransform = value;
            }
        }

        public void ApplyTransform(Matrix4 transform)
        {
            m_activeShader?.SetTransform(transform);
        }

        private void EnsureActive()
        {
            if (!IsCurrent && !MakeCurrent())
                throw new InvalidOperationException("Unable to activate context for shader activation");
        }
    }
}