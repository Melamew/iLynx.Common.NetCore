﻿#region LICENSE
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
using SFML.Graphics;
using SFML.System;

namespace iLynx.UI.Sfml.Rendering
{
    public abstract class Geometry : Drawable
    {
        private readonly List<Vertex> vertices = new List<Vertex>();

        public Vertex[] Vertices => vertices.ToArray();

        public PrimitiveType PrimitiveType { get; protected set; }

        protected virtual void AddVertex(Vector2f position, Color color)
        {
            AddVertex(new Vertex(position, color));
        }

        protected virtual void DeleteVertex(int index)
        {
            vertices.RemoveAt(index);
        }

        protected virtual void ClearVertices()
        {
            vertices.Clear();
        }

        protected virtual void AddVertex(params Vertex[] v)
        {
            vertices.AddRange(v);
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(Vertices, PrimitiveType, states);
        }
    }
}