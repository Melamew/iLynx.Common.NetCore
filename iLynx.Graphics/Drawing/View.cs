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
using OpenTK;

namespace iLynx.Graphics.Drawing
{
    public class View : Transformable, IView
    {
        private List<IDrawable> m_drawables = new List<IDrawable>();

        private Matrix4 m_projection = Matrix4.Identity;
        private Matrix4 m_viewTransform = Matrix4.Identity;

        public Matrix4 Projection
        {
            get => m_projection;
            set
            {
                if (value == m_projection) return;
                m_projection = value;
                m_viewTransform = Transform * m_projection;
            }
        }

        protected override void OnTransformChanged()
        {
            m_viewTransform = Transform * m_projection;
        }

        public void PrepareRender()
        {
            m_drawables = m_drawables.OrderBy(x => x.Shader).ThenBy(x => x.Texture).ToList();
        }

        public void Render(IRenderStates states)
        {
            states.ViewTransform = m_viewTransform;
            foreach (var drawable in m_drawables)
                drawable.Draw(states);
        }

        public void AddDrawable(IDrawable drawable)
        {
            m_drawables.Add(drawable);
        }

        public void RemoveDrawable(IDrawable drawable)
        {
            m_drawables.Remove(drawable);
        }
    }
}
