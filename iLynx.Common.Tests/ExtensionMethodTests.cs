using Xunit;
using iLynx.Common;

namespace iLynx.Common.Tests
{
    public class ExtensionMethodTests
    {
        [Fact]
        public void WhenToCalledThenCorrectResultReturned()
        {
            int[] expected = {1, 2, 3, 4, 5};
            var result = 1.To(5);
            Assert.Equal(5, result.Length);
            Assert.Equal(expected, result);
        }
    }
}
