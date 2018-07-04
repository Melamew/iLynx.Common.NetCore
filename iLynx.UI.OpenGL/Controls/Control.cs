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
using iLynx.Graphics;
using OpenTK;

namespace iLynx.UI.OpenGL.Controls
{
    // ReSharper disable once InconsistentNaming
    public abstract class Control : UIElement
    {
        private Color background = Color.Transparent;
        private Func<SizeF, Geometry> backgroundShapeGenerator = size => new RectangleGeometry(size, Color.Transparent, true);
        private Geometry shape;
        private SizeF size;

        public Func<SizeF, Geometry> BackgroundShapeGenerator
        {
            get => backgroundShapeGenerator;
            set
            {
                if (value == backgroundShapeGenerator) return;
                if (null == value) return;
                var old = backgroundShapeGenerator;
                backgroundShapeGenerator = value;
                OnPropertyChanged(old, value);
                OnLayoutPropertyChanged();
            }
        }

        protected Control(Alignment horizontalAlignment = Alignment.Start,
            Alignment verticalAlignment = Alignment.Start)
            : base(horizontalAlignment, verticalAlignment)
        {
        }

        protected override void UpdateLocked()
        {
            shape.FillColor = background;
        }

        public Color Background
        {
            get => background;
            set
            {
                if (value == background) return;
                var old = background;
                background = value;
                OnPropertyChanged(old, value);
            }
        }

        protected override RectangleF LayoutLocked(RectangleF finalRect)
        {
            if (finalRect.Size == size) return finalRect;
            size = finalRect.Size;
            shape = backgroundShapeGenerator(size);
            return finalRect;
        }

        protected override void DrawLocked(IDrawingContext target)
        {
            if (size == default(SizeF)) return;
            shape.FillColor = background;
            //target.Draw(shape);
        }

        public override bool HitTest(PointF position, out IInputElement element)
        {
            return base.HitTest(position, out element);
        }
    }
}
