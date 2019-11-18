using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace HardwareIntrinsics.Tests.Sse
{
    [TestClass]
    public class SseAddTests
    {
        [TestMethod]
        public void AddSingleVector128_ShouldWork()
        {
            var inputLeft = new float[] { 1f, 2f, 3f, 4f };
            var inputRight = new float[] { 4f, 3f, 2f, 1f };
            var expected = new float[] { 5f, 5f, 5f, 5f };

            var actual = SseInstructions.AddSingleVector128(inputLeft, inputRight);

            actual.ToArray().Should().Equal(expected);
        }

        [TestMethod]
        public void AddSingleVector128_With2Elements_ShouldWork()
        {
            var inputLeft = new float[] { 1f, 2f };
            var inputRight = new float[] { 4f, 3f };
            var expected = new float[] { 5f, 5f };

            var actual = SseInstructions.AddSingleVector128(inputLeft, inputRight);

            actual.ToArray().Should().Equal(expected);
        }

        [TestMethod]
        public void AddSingleVector128_With5Elements_ShouldThrowArgumentException()
        {
            var inputLeft = new float[] { 1f, 2f, 3f, 4f, 5f };
            var inputRight = new float[] { 4f, 3f, 2f, 1f, 0f };

            Action actual = () => SseInstructions.AddSingleVector128(inputLeft, inputRight);

            actual.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void AddSingleVector128_WithMismatchedLengths_ShouldThrowArgumentException()
        {
            var inputLeft = new float[] { 1f, 2f };
            var inputRight = new float[] { 4f, 3f, 2f, 1f };

            Action actual = () => SseInstructions.AddSingleVector128(inputLeft, inputRight);

            actual.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void Add_ShouldWork()
        {
            var inputLeft = new float[] { 1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f };
            var inputRight = new float[] { 4f, 3f, 2f, 1f, 0f, -1f, -2f, -3f, -4f };
            var expected = new float[] { 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f };

            var actual = SseInstructions.Add(inputLeft, inputRight);

            actual.ToArray().Should().Equal(expected);
        }

        [TestMethod]
        public void Add_WithMismatchedLengths_ShouldThrowArgumentException()
        {
            var inputLeft = new float[] { 1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f };
            var inputRight = new float[] { 4f, 3f, 2f, 1f, 0f };

            Action actual = () => SseInstructions.Add(inputLeft, inputRight);

            actual.Should().Throw<ArgumentException>();
        }
    }
}
