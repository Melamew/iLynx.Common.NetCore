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

using System.Collections.Generic;
using System.Linq;
using iLynx.Graphics.Shaders;
using OpenTK;
using OpenTK.Graphics;

namespace iLynx.Graphics
{
    public class View : Transformable, IView
    {
        private readonly IGraphicsContext context;

        private readonly List<IDrawable> drawables = new List<IDrawable>();

        private readonly List<RenderBatch> renderBatches = new List<RenderBatch>();

        public View(IGraphicsContext context)
        {
            this.context = context;
        }

        public Matrix4 Projection { get; set; } = Matrix4.Identity;

        public void PrepareRender()
        {
            // TODO: Only update when necessary, don't rebuild the entire thing.
            renderBatches.Clear();
            foreach (var drawable in drawables)
            {
                var call = drawable.CreateDrawCall();
                var batch = renderBatches.FirstOrDefault(x =>
                                x.Shader == drawable.Shader && x.Texture == drawable.Texture) ?? new RenderBatch
                                {
                                    Shader = drawable.Shader,
                                    Texture = drawable.Texture
                                };
                batch.AddCall(call);
                if (renderBatches.Contains(batch)) continue;
                renderBatches.Add(batch);
            }
        }

        public void Render()
        {
            foreach (var batch in renderBatches)
                batch.Execute(Transform * Projection);
            //throw new System.NotImplementedException();
        }

        public void AddDrawable(IDrawable drawable)
        {
            drawables.Add(drawable);
        }

        public void RemoveDrawable(IDrawable drawable)
        {
            throw new System.NotImplementedException();
        }
    }
}
