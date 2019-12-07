using System;
using System.Runtime.Intrinsics.X86;

namespace HardwareIntrinsics
{
    public class SseInstructions
    {
        /// <summary>
        /// Check if SSE instructions are supported on this machine
        /// </summary>
        public static bool IsSupported => Sse.IsSupported;

        /// <summary>
        /// Add two <see cref="ReadOnlySpan{float}"/> of the same length together
        /// </summary>
        /// <param name="left">Left side of addition</param>
        /// <param name="right">Right side of addition</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown when the length of <paramref name="left"/> and <paramref name="right"/> is not equal</exception>
        public static unsafe ReadOnlySpan<float> Add(ReadOnlySpan<float> left, ReadOnlySpan<float> right)
        {
            if (left.Length != right.Length)
            {
                throw new ArgumentException($"Cannot operate on two elements of different length ({left.Length} vs {right.Length})", nameof(left));
            }

            var length = Math.Min(left.Length, right.Length);
            var totalResult = new float[length];
            var stepSize = sizeof(float);

            for (int i = 0; i < length; i += stepSize)
            {
                var start = i;
                var end = Math.Min(start + stepSize, length);
                var range = new Range(start, end);

                var leftSpan = left[range];
                var rightSpan = right[range];
                var resultSpan = AddSingleVector128(leftSpan, rightSpan);

                for (int j = 0; (j < stepSize) && (i + j < length); j++)
                {
                    totalResult[i + j] = resultSpan[j];
                }
            }

            return totalResult;
        }

        /// <summary>
        /// Add two <see cref="ReadOnlySpan{float}"/> of the same length (max. 4) together
        /// </summary>
        /// <param name="leftSpan">Left side of addition</param>
        /// <param name="rightSpan">Right side of addition</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="leftSpan"/> and <paramref name="rightSpan"/> do not have the same length</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="leftSpan"/> or <paramref name="rightSpan"/> has more than 4 items</exception>
        public static unsafe ReadOnlySpan<float> AddSingleVector128(ReadOnlySpan<float> leftSpan, ReadOnlySpan<float> rightSpan)
        {
            if (leftSpan.Length != rightSpan.Length)
            {
                throw new ArgumentException($"Cannot operate on two elements of different length ({leftSpan.Length} vs {rightSpan.Length})", nameof(leftSpan));
            }

            if (leftSpan.Length > 4)
            {
                throw new ArgumentException("Cannot operate on anything larger than a 128 bit number (4 floats)", nameof(leftSpan));
            }

            if (rightSpan.Length > 4)
            {
                throw new ArgumentException("Cannot operate on anything larger than a 128 bit number (4 floats)", nameof(rightSpan));
            }

            fixed (float* leftAddress = leftSpan)
            fixed (float* rightAddress = rightSpan)
            {
                var leftVector = Sse.LoadVector128(leftAddress);
                var rightVector = Sse.LoadVector128(rightAddress);

                var result = Sse.Add(leftVector, rightVector);
                return new ReadOnlySpan<float>(&result, Math.Min(4, leftSpan.Length)).ToArray();
            }
        }
    }
}
