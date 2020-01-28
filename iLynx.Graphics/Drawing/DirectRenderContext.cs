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
using System.Collections.Generic;
using System.Linq;
using iLynx.Graphics.Animation;
using JetBrains.Annotations;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;
using static iLynx.Graphics.GLCheck;

namespace iLynx.Graphics.Drawing
{
    public abstract class RenderContext : IRenderContext
    {
        private readonly Queue<(Delegate Method, object[] Parameters)> m_pendingActions = new Queue<(Delegate, object[])>();
        protected readonly Dictionary<uint, IView> ViewCollection = new Dictionary<uint, IView>(10);
        private uint m_currentId = 1;
        private uint NextId => m_currentId++;
        public IAnimator Animator { get; } = new Animator();

        public abstract bool IsCurrent { get; }

        public abstract bool MakeCurrent();

        public void QueueForSync(Delegate method, params object[] parameters)
        {
            m_pendingActions.Enqueue((method, parameters));
        }

        public void ProcessSyncQueue(uint maxCalls = 0)
        {
            var calls = 0;
            while (m_pendingActions.TryDequeue(out var result) && (maxCalls == 0 || calls < maxCalls))
            {
                ++calls;
                result.Method.DynamicInvoke(result.Parameters);
            }
        }

        /// <inheritdoc />
        public uint AddView(IView view)
        {
            var key = NextId;
            ViewCollection.Add(key, view);
            return key;
        }

        /// <inheritdoc />
        public void RemoveView(IView view)
        {
            var kvp = ViewCollection.FirstOrDefault(x => x.Value == view);
            if (kvp.Value != view) return;
            ViewCollection.Remove(kvp.Key);
        }

        /// <inheritdoc />
        public void RemoveView(uint viewId)
        {
            ViewCollection.Remove(viewId);
        }

        /// <inheritdoc />
        public IReadOnlyCollection<IView> Views => ViewCollection.Values;

        protected virtual IEnumerable<IView> OrderedViews()
        {
            return ViewCollection.OrderBy(x => x.Key).Select(x => x.Value);
        }

        /// <inheritdoc />
        public void Update()
        {
            Animator.Tick();
            ProcessSyncQueue(5);
            if (m_pendingActions.Count > 10)
                ProcessSyncQueue();
            // TODO: See if there are any optimizations to rendering we can do here
        }

        public abstract void Render();
    }

    public class DirectRenderContext : RenderContext
    {
        private readonly IWindowInfo m_window;
        private readonly IGraphicsContext m_graphicsContext;
        private readonly IRenderStates m_renderStates = new RenderStates();

        public DirectRenderContext([NotNull]IGraphicsContext graphicsContext, [NotNull]IWindowInfo window)
        {
            m_graphicsContext = graphicsContext;
            m_window = window;
            if (!m_graphicsContext.IsCurrent)
                m_graphicsContext.MakeCurrent(m_window);
            Check(GL.Enable, EnableCap.Blend);
            Check(GL.BlendFunc, BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            Check(GL.Enable, EnableCap.CullFace);
            Check(GL.CullFace, CullFaceMode.Back);
            Check(GL.Enable, EnableCap.DepthTest);
            Check(GL.DepthFunc, DepthFunction.Less);
            Check(GL.Enable, EnableCap.StencilTest);
            Check(GL.StencilOp, StencilOp.Keep, StencilOp.Keep, StencilOp.Replace);
        }

        public override bool IsCurrent => m_graphicsContext.IsCurrent;

        public override bool MakeCurrent()
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

        /// <inheritdoc />
        public override void Render()
        {
            EnsureActive();
            GL.ClearColor(Color.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
            foreach (var view in OrderedViews())
                view.Render(m_renderStates);
            m_graphicsContext.SwapBuffers();
        }

        private void EnsureActive()
        {
            if (!IsCurrent && !MakeCurrent())
                throw new InvalidOperationException("Unable to activate context for shader activation");
        }
    }

    public class FrameBufferContext : RenderContext, IDisposable
    {
        private readonly int m_bufferId;
        private readonly Texture m_texture;

        public FrameBufferContext(uint width, uint height)
        {
            m_bufferId = Check(GL.GenFramebuffer);
            Check(GL.BindFramebuffer, FramebufferTarget.Framebuffer, m_bufferId);
            m_texture = Texture.Create(width, height, Color32.Lime);
            //Check(GL.FramebufferTexture2D(Frame))
            Check(GL.Viewport, 0, 0, (int)width, (int)height);
        }

        public override bool IsCurrent => Check(GL.GetInteger, GetPName.DrawFramebufferBinding) == m_bufferId;

        public override bool MakeCurrent()
        {
            bool isCurrent;
            try
            {
                Check(GL.BindFramebuffer, FramebufferTarget.Framebuffer, m_bufferId);
                isCurrent = IsCurrent;
                if (!isCurrent)
                    Console.WriteLine("Unable to set framebuffer as current");
            }
            catch (Exception e)
            {
                Console.WriteLine("Got error when attempting to make framebuffer current:");
                Console.WriteLine(e);
                isCurrent = false;
            }
            return isCurrent;
        }

        public override void Render()
        {
            if (!IsCurrent && !MakeCurrent())
                return;
            //foreach (var view in ViewCollection.OrderBy())

        }

        private void ReleaseUnmanagedResources()
        {
            Check(GL.DeleteFramebuffer, m_bufferId);
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~FrameBufferContext()
        {
            ReleaseUnmanagedResources();
        }
    }
}