using System;
using System.Collections.Generic;
using System.Text;
using iLynx.Graphics.Rendering;
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
