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
using iLynx.Graphics.Animation;
using iLynx.Graphics.Drawing;
using JetBrains.Annotations;
using OpenTK;

namespace iLynx.Graphics
{
    /// <summary>
    /// Defines a context that is used to render OpenGL content and synchronize off-loaded operations (Such as texture loading, etc.)
    /// </summary>
    public interface IRenderContext
    {
        /// <summary>
        /// Gets a value indicating whether or not this context is active on the current thread.
        /// </summary>
        bool IsCurrent { get; }
        /// <summary>
        /// Attempts to activate this context on the current thread and returns a value indicating whether or not it is.
        /// </summary>
        /// <returns></returns>
        bool MakeCurrent();
        /// <summary>
        /// Enqueues the specified method (with parameters) in the synchronization queue for this context
        /// </summary>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        void QueueForSync(Delegate method, params object[] parameters);
        /// <summary>
        /// Processes the synchronization queue of this context, up to a maximum calls / operations of <paramref name="maxCalls"/>
        /// <remarks>If <see cref="maxCalls"/> is 0, this method will attempt to process all pending sync calls</remarks>
        /// </summary>
        /// <param name="maxCalls">The maximum number of calls to process (0 = unlimited)</param>
        void ProcessSyncQueue(uint maxCalls = 0);
        /// <summary>
        /// Adds a view to be rendered in this context
        /// <remarks>Note that views are rendered in the order they are added</remarks>
        /// </summary>
        /// <param name="view"></param>
        /// <returns>A <see cref="uint"/> that can be used to identify the view within this context</returns>
        uint AddView(IView view);
        /// <summary>
        /// Removes a view from this context
        /// <remarks>Note that views are rendered in the order they are added</remarks>
        /// </summary>
        /// <param name="view"></param>
        void RemoveView(IView view);
        /// <summary>
        /// Removes the view with the specified id from this context
        /// <remarks>Note that views are rendered in the order they are added</remarks>
        /// </summary>
        /// <param name="viewId"></param>
        void RemoveView(uint viewId);
        /// <summary>
        /// Gets a read-only collection of views that are rendered in this context
        /// </summary>
        IReadOnlyCollection<IView> Views { get; }
        /// <summary>
        /// Gets the <see cref="IAnimator"/> associated with this context.
        /// <remarks>The animator will be called whenever <see cref="Update"/> is called on this <see cref="IRenderContext"/></remarks>
        /// </summary>
        IAnimator Animator { get; }
        /// <summary>
        /// Prepares this view for rendering
        /// </summary>
        void Update();
        /// <summary>
        /// Renders all the views contained in this context
        /// </summary>
        void Render();
        /// <summary>
        /// Renders the view with the specified id
        /// </summary>
        /// <param name="viewId">The id of the view to render</param>
        void Render(uint viewId);
    }

    /// <summary>
    /// Defines a set of variables that are passed from object to object during a render pass
    /// </summary>
    public interface IRenderStates
    {
        /// <summary>
        /// Gets or Sets the currently active shader
        /// </summary>
        Shader Shader { get; set; }
        /// <summary>
        /// Gets or Sets the currently active texture
        /// </summary>
        Texture Texture { get; set; }
        /// <summary>
        /// Gets or Sets the view transform that is applied to subsequent transformations when rendering
        /// </summary>
        Matrix4 ViewTransform { get; set; }
        /// <summary>
        /// Applies the specified transform on top of the <see cref="ViewTransform"/> and uploads the final value to the shader
        /// </summary>
        /// <param name="transform"></param>
        void ApplyTransform(Matrix4 transform);
    }

    public class RenderStates : IRenderStates
    {
        private Shader m_activeShader = Shader.DefaultShader;
        private Texture m_activeTexture = Texture.DefaultTexture;
        private Matrix4 m_viewTransform = Matrix4.Identity;

        public RenderStates()
        {
            Init();
        }

        public RenderStates(Shader shader, Texture texture, Matrix4 viewTransform)
        {
            m_activeShader = shader;
            m_activeTexture = texture;
            m_viewTransform = viewTransform;
            Init();
        }

        public RenderStates(Shader shader, Texture texture)
        {
            m_activeShader = shader;
            m_activeTexture = texture;
            Init();
        }

        public RenderStates(Shader shader)
        {
            m_activeShader = shader;
            Init();
        }

        public RenderStates(IRenderStates states)
        {
            m_activeShader = states.Shader;
            m_activeTexture = states.Texture;
            m_viewTransform = states.ViewTransform;
            Init();
        }

        private void Init()
        {
            Shader.Activate(m_activeShader);
            Texture.Bind(m_activeTexture);
        }

        /// <summary>
        /// Gets or Sets the currently active shader program
        /// </summary>
        public Shader Shader
        {
            get => m_activeShader;
            set => SetShader(value ?? Shader.DefaultShader);
        }

        private void SetShader([NotNull]Shader shader)
        {
            if (shader == m_activeShader) return;
            m_activeShader = shader;
            shader.ViewTransform = m_viewTransform;
            Shader.Activate(shader);
        }

        /// <summary>
        /// Gets or Sets the currently active texture
        /// </summary>
        public Texture Texture
        {
            get => m_activeTexture;
            set => SetTexture(value ?? Texture.DefaultTexture);
        }

        private void SetTexture([NotNull]Texture texture)
        {
            if (texture == m_activeTexture) return;
            m_activeTexture = texture;
            Texture.Bind(texture);
        }

        public Matrix4 ViewTransform
        {
            get => m_viewTransform;
            set
            {
                if (value == m_viewTransform) return;
                m_viewTransform = value;
                m_activeShader.ViewTransform = value;
            }
        }

        /// <inheritdoc/>
        public void ApplyTransform(Matrix4 transform)
        {
            m_activeShader.SetTransform(transform);
        }
    }
}