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
using System;
using iLynx.Graphics.Rendering;
using iLynx.Graphics.Rendering.Geometry;
using OpenTK;
using Xunit;

namespace iLynx.Graphics.Tests
{
    public class VertexBufferTests
    {
        [Fact]
        public void WhenSetVerticesCalledWithNullExceptionIsThrown()
        {
            var buffer = new VertexBuffer<float>();
            Assert.Throws<ArgumentNullException>(() => buffer.SetVertices(null));
        }

        [Fact]
        public void WhenReplaceVerticesCalledAndTotalLengthGreaterThanSizeThenBufferIsResized()
        {
            const int initialCapacity = 10;
            const int insertAmount = 10;
            const int offset = 10;
            const int expectedNewLength = offset + insertAmount;
            var buffer = new VertexBuffer<float>(initialCapacity);
            buffer.ReplaceVertices(10, new float[insertAmount]);
            Assert.Equal(expectedNewLength, buffer.Length);
        }

        [Fact]
        public void WhenSettingIndexThenGettingValueIsEqual()
        {
            var expected = new Vertex2(new Vector2(1f, 2f), Color.Green, new Vector2(3f, 4f));
            var buffer = new VertexBuffer<Vertex2>(10) {[4] = expected};
            Assert.Equal(expected, buffer[4]);
        }

        [Fact]
        public void WhenSettingIndexGreaterThanLengthExceptionIsThrown()
        {
            var buffer = new VertexBuffer<float>();
            Assert.Throws<IndexOutOfRangeException>(() => buffer[10] = 20f);
        }
    }
}