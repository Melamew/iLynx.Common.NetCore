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
        private readonly IRenderContext context;
        private List<IDrawable> drawables = new List<IDrawable>();

        private Matrix4 projection = Matrix4.Identity;
        private Matrix4 viewTransform = Matrix4.Identity;

        public View(IRenderContext context)
        {
            this.context = context;
            if (!context.IsInitialized)
                context.Initialize();
        }

        public Matrix4 Projection
        {
            get => projection;
            set
            {
                if (value == projection) return;
                projection = value;
                viewTransform = Transform * projection;
            }
        }

        protected override void OnTransformChanged()
        {
            viewTransform = Transform * projection;
        }

        public void PrepareRender()
        {
            drawables = drawables.OrderBy(x => x.Shader).ThenBy(x => x.Texture).ToList();
        }

        public void Render()
        {
            if (!context.IsCurrent && !context.MakeCurrent()) throw new InvalidOperationException("Context is not active");
            foreach (var drawable in drawables)
            {
                context.ViewTransform = viewTransform;
                drawable.Draw(context);
            }
        }

        public void AddDrawable(IDrawable drawable)
        {
            drawables.Add(drawable);
        }

        public void RemoveDrawable(IDrawable drawable)
        {
            drawables.Remove(drawable);
        }
    }
}
