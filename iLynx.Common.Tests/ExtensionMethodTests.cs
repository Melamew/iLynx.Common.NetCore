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
