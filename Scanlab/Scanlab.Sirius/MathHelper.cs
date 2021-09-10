// Decompiled with JetBrains decompiler
// Type: SpiralLab.Sirius.MathHelper
// Assembly: spirallab.sirius, Version=1.0.7.3, Culture=neutral, PublicKeyToken=null
// MVID: 45F5C82C-CD8C-4777-BF82-85C50A80042A
// Assembly location: C:\Users\sean0\Downloads\sirius-master\sirius-master\bin\spirallab.sirius.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Scanlab.Sirius
{
    /// <summary>수학 유틸리티</summary>
    internal static class MathHelper
    {
        /// <summary>
        /// Constant to transform an angle between degrees and radians.
        /// </summary>
        public const float DegToRad = 0.01745329f;
        /// <summary>
        /// Constant to transform an angle between degrees and radians.
        /// </summary>
        public const float RadToDeg = 57.29578f;
        private static float epsilon = 1E-06f;

        /// <summary>
        /// Represents the smallest number used for comparison purposes.
        /// </summary>
        /// <remarks>
        /// The epsilon value must be a positive number greater than zero.
        /// </remarks>
        internal static float Epsilon
        {
            get => MathHelper.epsilon;
            set => MathHelper.epsilon = (double)value > 0.0 ? value : throw new ArgumentOutOfRangeException(nameof(value), (object)value, "The epsilon value must be a positive number greater than zero.");
        }

        /// <summary>
        /// Returns a value indicating the sign of a double-precision floating-point number.
        /// </summary>
        /// <param name="number">Double precision number.</param>
        /// <returns>
        /// A number that indicates the sign of value.
        /// Return value Meaning -1 value is less than zero.
        /// 0 value is equal to zero.
        /// 1 value is greater than zero.
        /// </returns>
        /// <remarks>This method will test for values of numbers very close to zero.</remarks>
        internal static int Sign(float number) => !MathHelper.IsZero(number) ? Math.Sign(number) : 0;

        /// <summary>
        /// Returns a value indicating the sign of a double-precision floating-point number.
        /// </summary>
        /// <param name="number">Double precision number.
        /// <param name="threshold">Tolerance.</param>
        /// </param>
        /// <returns>
        /// A number that indicates the sign of value.
        /// Return value Meaning -1 value is less than zero.
        /// 0 value is equal to zero.
        /// 1 value is greater than zero.
        /// </returns>
        /// <remarks>This method will test for values of numbers very close to zero.</remarks>
        internal static int Sign(float number, float threshold) => !MathHelper.IsZero(number, threshold) ? Math.Sign(number) : 0;

        /// <summary>Checks if a number is close to one.</summary>
        /// <param name="number">Double precision number.</param>
        /// <returns>True if its close to one or false in any other case.</returns>
        internal static bool IsOne(float number) => MathHelper.IsOne(number, MathHelper.Epsilon);

        /// <summary>Checks if a number is close to one.</summary>
        /// <param name="number">Double precision number.</param>
        /// <param name="threshold">Tolerance.</param>
        /// <returns>True if its close to one or false in any other case.</returns>
        internal static bool IsOne(float number, float threshold) => MathHelper.IsZero(number - 1f, threshold);

        /// <summary>Checks if a number is close to zero.</summary>
        /// <param name="number">Double precision number.</param>
        /// <returns>True if its close to one or false in any other case.</returns>
        public static bool IsZero(float number) => MathHelper.IsZero(number, MathHelper.Epsilon);

        public static bool IsZero(double number) => MathHelper.IsZero(number, (double)MathHelper.Epsilon);

        /// <summary>Checks if a number is close to zero.</summary>
        /// <param name="number">Double precision number.</param>
        /// <param name="threshold">Tolerance.</param>
        /// <returns>True if its close to one or false in any other case.</returns>
        public static bool IsZero(float number, float threshold) => (double)number >= -(double)threshold && (double)number <= (double)threshold;

        public static bool IsZero(double number, double threshold) => number >= -threshold && number <= threshold;

        /// <summary>Checks if a number is equal to another.</summary>
        /// <param name="a">Double precision number.</param>
        /// <param name="b">Double precision number.</param>
        /// <returns>True if its close to one or false in any other case.</returns>
        public static bool IsEqual(float a, float b) => MathHelper.IsEqual(a, b, MathHelper.Epsilon);

        public static bool IsEqual(double a, double b) => MathHelper.IsEqual(a, b, (double)MathHelper.Epsilon);

        /// <summary>Checks if a number is equal to another.</summary>
        /// <param name="a">Double precision number.</param>
        /// <param name="b">Double precision number.</param>
        /// <param name="threshold">Tolerance.</param>
        /// <returns>True if its close to one or false in any other case.</returns>
        public static bool IsEqual(float a, float b, float threshold) => MathHelper.IsZero(a - b, threshold);

        public static bool IsEqual(double a, double b, double threshold) => MathHelper.IsZero(a - b, threshold);

        /// <summary>
        /// Normalizes the value of an angle in degrees between [0, 360[.
        /// </summary>
        /// <param name="angle">Angle in degrees.</param>
        /// <returns>The equivalent angle in the range [0, 360[.</returns>
        /// <remarks>Negative angles will be converted to its positive equivalent.</remarks>
        public static float NormalizeAngle(float angle)
        {
            float number = angle % 360f;
            if (MathHelper.IsZero(number))
                number = 0.0f;
            return (double)number < 0.0 ? 360f + number : number;
        }

        /// <summary>두 점간 거리</summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        internal static float PointLength(double x1, double y1, double x2, double y2) => (float)Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));

        /// <summary>점이 원안에 포함되는지 여부</summary>
        /// <param name="px"></param>
        /// <param name="py"></param>
        /// <param name="cx"></param>
        /// <param name="cy"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        internal static bool IntersectPointInCircle(
          double px,
          double py,
          double cx,
          double cy,
          double radius)
        {
            return (double)MathHelper.PointLength(px, py, cx, cy) <= radius;
        }

        /// <summary>점이 사각형 영역 내에 포함되어 있는지 여부</summary>
        /// <param name="br"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        internal static bool IntersectPointInRect(BoundRect br, double x, double y, double limit = 0.05) => !br.IsEmpty && (double)br.Left - limit <= x && ((double)br.Right + limit >= x && (double)br.Bottom - limit <= y) && (double)br.Top + limit >= y;

        /// <summary>점이 직선과 교차(충돌) 혹은 근접하는지 여부</summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="px"></param>
        /// <param name="py"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        internal static bool IntersectPointInLine(
          double x1,
          double y1,
          double x2,
          double y2,
          double px,
          double py,
          double limit = 0.05)
        {
            double num1 = (double)MathHelper.PointLength(px, py, x1, y1);
            double num2 = (double)MathHelper.PointLength(px, py, x2, y2);
            double num3 = (double)MathHelper.PointLength(x1, y1, x2, y2);
            return num1 + num2 >= num3 - limit && num1 + num2 <= num3 + limit;
        }

        /// <summary>직선이 원과 교차(충돌) 혹은 근접하는지 여부</summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="cx"></param>
        /// <param name="cy"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        internal static bool IntersectLineInCircle(
          double x1,
          double y1,
          double x2,
          double y2,
          double cx,
          double cy,
          double radius = 0.05)
        {
            if (MathHelper.IntersectPointInCircle(x1, y1, cx, cy, radius) | MathHelper.IntersectPointInCircle(x2, y2, cx, cy, radius))
                return true;
            double x = (double)MathHelper.PointLength(x1, y1, x2, y2);
            double num1 = ((cx - x1) * (x2 - x1) + (cy - y1) * (y2 - y1)) / Math.Pow(x, 2.0);
            double px = x1 + num1 * (x2 - x1);
            double py = y1 + num1 * (y2 - y1);
            if (!MathHelper.IntersectPointInLine(x1, y1, x2, y2, px, py))
                return false;
            double num2 = px - cx;
            double num3 = py - cy;
            return Math.Sqrt(num2 * num2 + num3 * num3) <= radius;
        }

        /// <summary>직선과 직선이 교차(충돌) 하는지 여부</summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="x3"></param>
        /// <param name="y3"></param>
        /// <param name="x4"></param>
        /// <param name="y4"></param>
        /// <returns></returns>
        internal static bool IntersectLineInLine(
          double x1,
          double y1,
          double x2,
          double y2,
          double x3,
          double y3,
          double x4,
          double y4)
        {
            return MathHelper.IntersectLineInLine(x1, y1, x2, y2, x3, y3, x4, y4, out double _, out double _);
        }

        /// <summary>직선과 직선이 교차(충돌) 하는지 여부 (교차점 구하기)</summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="x3"></param>
        /// <param name="y3"></param>
        /// <param name="x4"></param>
        /// <param name="y4"></param>
        /// <param name="collisionX"></param>
        /// <param name="collisionY"></param>
        /// <returns></returns>
        internal static bool IntersectLineInLine(
          double x1,
          double y1,
          double x2,
          double y2,
          double x3,
          double y3,
          double x4,
          double y4,
          out double collisionX,
          out double collisionY)
        {
            collisionX = collisionY = 0.0;
            if ((y4 - y3) * (x2 - x1) == (x4 - x3) * (y2 - y1) || (y4 - y3) * (x2 - x1) == (x4 - x3) * (y2 - y1))
                return false;
            double num1 = ((x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));
            double num2 = ((x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));
            if (num1 < 0.0 || num1 > 1.0 || (num2 < 0.0 || num2 > 1.0))
                return false;
            collisionX = x1 + num1 * (x2 - x1);
            collisionY = y1 + num1 * (y2 - y1);
            return true;
        }

        /// <summary>직선이 사각형 영역 내에 있던가, 혹은 모서리의 일부가 교차(포함)하는지 여부</summary>
        /// <param name="br"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        internal static bool IntersectLineInRect(
          BoundRect br,
          double x1,
          double y1,
          double x2,
          double y2)
        {
            return !br.IsEmpty && (((double)br.Left > x1 || x1 > (double)br.Right || ((double)br.Bottom > y1 || y1 > (double)br.Top) || ((double)br.Left > x2 || x2 > (double)br.Right || (double)br.Bottom > y2) ? 0 : (y2 <= (double)br.Top ? 1 : 0)) != 0 || MathHelper.IntersectLineInLine(x1, y1, x2, y2, (double)br.Left, (double)br.Top, (double)br.Left, (double)br.Bottom, out double _, out double _) || (MathHelper.IntersectLineInLine(x1, y1, x2, y2, (double)br.Right, (double)br.Top, (double)br.Right, (double)br.Bottom, out double _, out double _) || MathHelper.IntersectLineInLine(x1, y1, x2, y2, (double)br.Left, (double)br.Top, (double)br.Right, (double)br.Top, out double _, out double _)) || MathHelper.IntersectLineInLine(x1, y1, x2, y2, (double)br.Left, (double)br.Bottom, (double)br.Right, (double)br.Bottom, out double _, out double _));
        }

        /// <summary>직선이 사각형 영역 내에 있던가, 혹은 모서리의 일부가 교차(포함)하는지 여부</summary>
        /// <param name="br"></param>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="endX"></param>
        /// <param name="endY"></param>
        /// <param name="collisions">충돌(교차)시 해당 좌표 배열 (하나 혹은 둘)</param>
        /// <returns></returns>
        internal static bool IntersectLineInRectWithCollision(
          BoundRect br,
          double startX,
          double startY,
          double endX,
          double endY,
          out List<Vector2> collisions)
        {
            collisions = new List<Vector2>();
            List<Vector2> source = new List<Vector2>();
            if (br.IsEmpty)
                return false;
            if (((double)br.Left > startX || startX > (double)br.Right || ((double)br.Bottom > startY || startY > (double)br.Top) || ((double)br.Left > endX || endX > (double)br.Right || (double)br.Bottom > endY) ? 0 : (endY <= (double)br.Top ? 1 : 0)) != 0)
                return true;
            double collisionX1;
            double collisionY1;
            if (MathHelper.IntersectLineInLine(startX, startY, endX, endY, (double)br.Left, (double)br.Top, (double)br.Left, (double)br.Bottom, out collisionX1, out collisionY1))
                source.Add(new Vector2((float)collisionX1, (float)collisionY1));
            double collisionX2;
            double collisionY2;
            if (MathHelper.IntersectLineInLine(startX, startY, endX, endY, (double)br.Right, (double)br.Top, (double)br.Right, (double)br.Bottom, out collisionX2, out collisionY2))
                source.Add(new Vector2((float)collisionX2, (float)collisionY2));
            double collisionX3;
            double collisionY3;
            if (MathHelper.IntersectLineInLine(startX, startY, endX, endY, (double)br.Left, (double)br.Top, (double)br.Right, (double)br.Top, out collisionX3, out collisionY3))
                source.Add(new Vector2((float)collisionX3, (float)collisionY3));
            double collisionX4;
            double collisionY4;
            if (MathHelper.IntersectLineInLine(startX, startY, endX, endY, (double)br.Left, (double)br.Bottom, (double)br.Right, (double)br.Bottom, out collisionX4, out collisionY4))
                source.Add(new Vector2((float)collisionX4, (float)collisionY4));
            collisions = source.Distinct<Vector2>().ToList<Vector2>();
            if (2 == collisions.Count && !MathHelper.IsEqual(Vertex.Angle(new Vertex((float)startX, (float)startY), new Vertex((float)endX, (float)endY)), Vertex.Angle(new Vertex(collisions[0].X, collisions[0].Y), new Vertex(collisions[1].X, collisions[1].Y))))
            {
                Vector2 vector2 = collisions[0];
                collisions[0] = collisions[1];
                collisions[1] = vector2;
            }
            return collisions.Count > 0;
        }

        /// <summary>두 사각형이 일부라도 교차(교집합) 혹은 포함되는지 여부</summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        internal static bool IntersectRectInRect(BoundRect a, BoundRect b, double limit = 0.05) => MathHelper.IntersectPointInRect(a, (double)b.Left, (double)b.Top, limit) || MathHelper.IntersectPointInRect(a, (double)b.Right, (double)b.Top, limit) || (MathHelper.IntersectPointInRect(a, (double)b.Left, (double)b.Bottom, limit) || MathHelper.IntersectPointInRect(a, (double)b.Right, (double)b.Bottom, limit)) || (MathHelper.IntersectPointInRect(b, (double)a.Left, (double)a.Top, limit) || MathHelper.IntersectPointInRect(b, (double)a.Right, (double)a.Top, limit) || (MathHelper.IntersectPointInRect(b, (double)a.Left, (double)a.Bottom, limit) || MathHelper.IntersectPointInRect(b, (double)a.Right, (double)a.Bottom, limit)));

        /// <summary>두 사각형의 외곽 선분들중 일부라도 교차(교집합) 여부 (포함은 제외됨)</summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        internal static bool CollisionRectWithRect(BoundRect a, BoundRect b)
        {
            if (a.IsEmpty || b.IsEmpty)
                return false;
            return MathHelper.IntersectLineInLine((double)a.Left, (double)a.Top, (double)a.Left, (double)a.Bottom, (double)b.Left, (double)b.Top, (double)b.Left, (double)b.Bottom) || MathHelper.IntersectLineInLine((double)a.Left, (double)a.Top, (double)a.Left, (double)a.Bottom, (double)b.Right, (double)b.Top, (double)b.Right, (double)b.Bottom) || (MathHelper.IntersectLineInLine((double)a.Left, (double)a.Top, (double)a.Left, (double)a.Bottom, (double)b.Left, (double)b.Top, (double)b.Right, (double)b.Top) || MathHelper.IntersectLineInLine((double)a.Left, (double)a.Top, (double)a.Left, (double)a.Bottom, (double)b.Left, (double)b.Bottom, (double)b.Right, (double)b.Bottom)) || (MathHelper.IntersectLineInLine((double)a.Right, (double)a.Top, (double)a.Right, (double)a.Bottom, (double)b.Left, (double)b.Top, (double)b.Left, (double)b.Bottom) || MathHelper.IntersectLineInLine((double)a.Right, (double)a.Top, (double)a.Right, (double)a.Bottom, (double)b.Right, (double)b.Top, (double)b.Right, (double)b.Bottom) || (MathHelper.IntersectLineInLine((double)a.Right, (double)a.Top, (double)a.Right, (double)a.Bottom, (double)b.Left, (double)b.Top, (double)b.Right, (double)b.Top) || MathHelper.IntersectLineInLine((double)a.Right, (double)a.Top, (double)a.Right, (double)a.Bottom, (double)b.Left, (double)b.Bottom, (double)b.Right, (double)b.Bottom))) || (MathHelper.IntersectLineInLine((double)a.Left, (double)a.Top, (double)a.Right, (double)a.Top, (double)b.Left, (double)b.Top, (double)b.Left, (double)b.Bottom) || MathHelper.IntersectLineInLine((double)a.Left, (double)a.Top, (double)a.Right, (double)a.Top, (double)b.Right, (double)b.Top, (double)b.Right, (double)b.Bottom) || (MathHelper.IntersectLineInLine((double)a.Left, (double)a.Top, (double)a.Right, (double)a.Top, (double)b.Left, (double)b.Top, (double)b.Right, (double)b.Top) || MathHelper.IntersectLineInLine((double)a.Left, (double)a.Top, (double)a.Right, (double)a.Top, (double)b.Left, (double)b.Bottom, (double)b.Right, (double)b.Bottom)) || (MathHelper.IntersectLineInLine((double)a.Left, (double)a.Bottom, (double)a.Right, (double)a.Bottom, (double)b.Left, (double)b.Top, (double)b.Left, (double)b.Bottom) || MathHelper.IntersectLineInLine((double)a.Left, (double)a.Bottom, (double)a.Right, (double)a.Bottom, (double)b.Right, (double)b.Top, (double)b.Right, (double)b.Bottom) || MathHelper.IntersectLineInLine((double)a.Left, (double)a.Bottom, (double)a.Right, (double)a.Bottom, (double)b.Left, (double)b.Top, (double)b.Right, (double)b.Top))) || MathHelper.IntersectLineInLine((double)a.Left, (double)a.Bottom, (double)a.Right, (double)a.Bottom, (double)b.Left, (double)b.Bottom, (double)b.Right, (double)b.Bottom);
        }
    }
}
