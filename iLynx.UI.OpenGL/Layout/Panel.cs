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
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using iLynx.Graphics;
using iLynx.UI.OpenGL.Controls;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace iLynx.UI.OpenGL.Layout
{
    public abstract class Panel : UIElement
    {
        private readonly List<IUIElement> children = new List<IUIElement>();
        public IEnumerable<IUIElement> Children => children;
        private Color background = Color.Transparent;
        private volatile bool requireNewTexture = true;

        protected Panel(Alignment horizontalAlignment = Alignment.Stretch, Alignment verticalAlignment = Alignment.Stretch)
            : base(horizontalAlignment, verticalAlignment)
        {
            IsFocusable = false;
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

        protected override void OnLayoutPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnLayoutPropertyChanged(propertyName);
        }

        public void AddChild(params IUIElement[] elements)
        {
            children.AddRange(elements.Select(x =>
            {
                x.SetLogicalParent(this);
                x.LayoutPropertyChanged += OnChildLayoutPropertyChanged;
                return x;
            }));
            OnLayoutPropertyChanged(nameof(Children));
        }

        protected virtual void OnChildLayoutPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var desiredSize = Measure(RenderSize);
            if (desiredSize != RenderSize)
                OnLayoutPropertyChanged($"{sender}.{e.PropertyName}");
            else
                Layout(BoundingBox + Margin);
        }

        public void RemoveChild(params IUIElement[] elements)
        {
            foreach (var element in elements)
            {
                element.LayoutPropertyChanged -= OnChildLayoutPropertyChanged;
                children.Remove(element);
                element.SetLogicalParent(null);
            }
            OnLayoutPropertyChanged(nameof(Children));
        }

        //private (RenderTexture texture, Sprite sprite) GetRenderItems()
        //{
        //    if (!requireNewTexture || textureDimensions.X <= 0 || textureDimensions.Y <= 0) return (texture, sprite);
        //    texture?.Dispose();
        //    texture = null;
        //    texture = new RenderTexture(textureDimensions.X, textureDimensions.Y);
        //    sprite = new Sprite(texture.Texture);
        //    requireNewTexture = false;
        //    return (texture, sprite);
        //}

        protected override void UpdateLocked()
        {
            //throw new NotImplementedException();
        }

        protected override void DrawLocked(IDrawingContext target)
        {
            //var renderItems = GetRenderItems();
            //var t = renderItems.texture;
            //if (null == t) return;
            //var s = renderItems.sprite;
            //var c = children.ToArray();
            //t.Clear(background);
            //var childStates = new RenderStates(states) { Transform = Transform.Identity };
            //foreach (var child in c)
            //    child.Draw(t, childStates);
            //t.Display();
            //target.Draw(s, states);
        }

        protected override RectangleF LayoutLocked(RectangleF finalRect)
        {
            return finalRect;
            //var dimensions = (Vector2u)finalRect.Size();
            //requireNewTexture = null == texture || textureDimensions != dimensions;
            //textureDimensions = dimensions;
            //LayoutChildren(new RectangleF(0f, 0f, dimensions.X, dimensions.Y));
            //return finalRect;
        }

        public override bool HitTest(PointF position, out IInputElement element)
        {
            var hit = base.HitTest(position, out element);
            if (!hit && IsHitTestVisible) return false;
            foreach (var child in children)
            {
                if (!child.HitTest(position, out var e)) continue;
                element = e;
                return true;
            }

            element = this;
            return IsHitTestVisible;
        }

        protected abstract void LayoutChildren(RectangleF target);
    }
}
