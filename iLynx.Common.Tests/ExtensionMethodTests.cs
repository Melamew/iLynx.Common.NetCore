using System;
using Xunit;
using iLynx.Common;

namespace iLynx.Common.Tests
{
    public class ExtensionMethodTests
    {
        [Fact]
        public void WhenToCalledWithPositiveRangeThenCorrectArrayReturned()
        {
            int[] expected = { 11, 12, 13, 14, 15 };
            var result = 11.To(15);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void WhenToCalledWithNegativeRangeThenCorrectArrayReturned()
        {
            int[] expected = { -11, -12, -13, -14, -15 };
            var result = (-11).To(-15);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void WhenToCalledWithPositiveInverseRangeThenCorrectArrayReturned()
        {
            int[] expected = { 15, 14, 13, 12, 11 };
            var result = 15.To(11);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void WhenToCalledWithNegativeInverseRangeThenCorrectArrayReturned()
        {
            int[] expected = { -15, -14, -13, -12, -11 };
            var result = (-15).To(-11);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void WhenUnsignedToCalledWithPositiveRangeThenCorrectArrayReturned()
        {
            uint[] expected = { 11, 12, 13, 14, 15 };
            var result = 11u.To(15);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void WhenUnsignedToCalledWithNegativeRangeThenCorrectArrayReturned()
        {
            uint[] expected = { 15, 14, 13, 12, 11 };
            var result = 15u.To(11);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void WhenToCalledWithZeroRangeThenFirstValueReturned()
        {
            int[] expected = { 10 };
            var result = 10.To(10);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void WhenToCalledWithNegativeZeroRangeThenFirstValueReturned()
        {
            int[] expected = { -10 };
            var result = (-10).To(-10);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void WhenUnsignedToCalledWithZeroRangeThenFirstValueReturned()
        {
            uint[] expected = { 10u };
            var result = 10u.To(10u);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void WhenToCalledWithPositiveToNegativeRangeThenCorrectArrayReturned()
        {
            int[] expected = { 5, 4, 3, 2, 1, 0, -1, -2, -3, -4, -5 };
            var result = 5.To(-5);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void WhenToCalledWithNegativeToPositiveRangeThenCorrectArrayReturned()
        {
            int[] expected = { -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5 };
            var result = (-5).To(5);
            Assert.Equal(expected, result);
        }
    }
}
