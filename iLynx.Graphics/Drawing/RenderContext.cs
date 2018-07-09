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

namespace iLynx.Graphics.Drawing
{
    public class RenderContext : IRenderContext
    {
        private readonly Queue<(Delegate Method, object[] Parameters)> m_pendingActions = new Queue<(Delegate, object[])>();
        private readonly Dictionary<uint, IView> m_views = new Dictionary<uint, IView>(10);
        private readonly IAnimator m_animator = new Animator();
        private readonly IWindowInfo m_window;
        private readonly IGraphicsContext m_graphicsContext;
        private readonly IRenderStates m_renderStates = new RenderStates();

        private uint m_currentId = 1;
        private uint NextId => m_currentId++;

        public RenderContext([NotNull]IGraphicsContext graphicsContext, [NotNull]IWindowInfo window)
        {
            m_graphicsContext = graphicsContext;
            m_window = window;
            if (!m_graphicsContext.IsCurrent)
                m_graphicsContext.MakeCurrent(m_window);
            GLCheck.Check(GL.Enable, EnableCap.Blend);
            GLCheck.Check(GL.BlendFunc, BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GLCheck.Check(GL.Enable, EnableCap.CullFace);
            GLCheck.Check(GL.CullFace, CullFaceMode.Back);
            GLCheck.Check(GL.Enable, EnableCap.DepthTest);
            GLCheck.Check(GL.DepthFunc, DepthFunction.Less);
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

        public IAnimator Animator => m_animator;

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
            m_views.Add(key, view);
            return key;
        }

        /// <inheritdoc />
        public void RemoveView(IView view)
        {
            var kvp = m_views.FirstOrDefault(x => x.Value == view);
            if (kvp.Value != view) return;
            m_views.Remove(kvp.Key);
        }

        /// <inheritdoc />
        public void RemoveView(uint viewId)
        {
            m_views.Remove(viewId);
        }

        /// <inheritdoc />
        public IReadOnlyCollection<IView> Views => m_views.Values;

        /// <inheritdoc />
        public void Update()
        {
            m_animator.Tick();
            // TODO: See if there are any optimizations to rendering we can do here
        }

        /// <inheritdoc />
        public void Render()
        {
            EnsureActive();
            GL.ClearColor(Color.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            foreach (var view in m_views.OrderBy(x => x.Key).Select(x => x.Value))
                view.Render(m_renderStates);
            //m_graphicsContext.SwapBuffers();
        }

        /// <inheritdoc />
        public void Render(uint viewId)
        {
        }

        private void EnsureActive()
        {
            if (!IsCurrent && !MakeCurrent())
                throw new InvalidOperationException("Unable to activate context for shader activation");
        }
    }
}