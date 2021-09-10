
using System;

namespace Scanlab.Sirius
{
    internal class MathHelper
    {
        /// <summary>각도 -&gt; 라디안 변환 상수</summary>
        public const float DegToRad = 0.01745329f;
        /// <summary>라디안 -&gt; 각도 변환 상수</summary>
        public const float RadToDeg = 57.29578f;
        /// <summary>마진값 상수</summary>
        private static float epsilon = 1E-06f;

        /// <summary>부동 소수점 동일성 비교</summary>
        /// <param name="number">부동소수점 값</param>
        /// <param name="threshold">마진값</param>
        /// <returns></returns>
        public static bool IsZero(float number, float threshold) => (double)number >= -(double)threshold && (double)number <= (double)threshold;

        /// <summary>부동 소수점 동일성 비교</summary>
        /// <param name="a">부동소수점 값</param>
        /// <param name="b">부동소수점 값</param>
        /// <returns></returns>
        public static bool IsEqual(float a, float b) => MathHelper.IsEqual(a, b, MathHelper.Epsilon);

        /// <summary>부동 소수점 동일성 비교</summary>
        /// <param name="a">부동소수점 값</param>
        /// <param name="b">부동소수점 값</param>
        /// <param name="threshold">마진값</param>
        /// <returns></returns>
        public static bool IsEqual(float a, float b, float threshold) => MathHelper.IsZero(a - b, threshold);

        /// <summary>마진값 상수</summary>
        internal static float Epsilon
        {
            get => MathHelper.epsilon;
            set => MathHelper.epsilon = (double)value > 0.0 ? value : throw new ArgumentOutOfRangeException(nameof(value), (object)value, "The epsilon value must be a positive number greater than zero.");
        }

        public static float CONIC_B1(float t) => (float)((1.0 - (double)t) * (1.0 - (double)t));

        public static float CONIC_B2(float t) => (float)(2.0 * (double)t * (1.0 - (double)t));

        public static float CONIC_B3(float t) => t * t;

        public static float CUBIC_B1(float t) => (float)((1.0 - (double)t) * (1.0 - (double)t) * (1.0 - (double)t));

        public static float CUBIC_B2(float t) => (float)(3.0 * (double)t * (1.0 - (double)t) * (1.0 - (double)t));

        public static float CUBIC_B3(float t) => (float)(3.0 * (double)t * (double)t * (1.0 - (double)t));

        public static float CUBIC_B4(float t) => t * t * t;
    }
}
