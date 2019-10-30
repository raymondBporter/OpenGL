using System;
using System.Diagnostics;
using TkRenderer.Math.LinearAlgebra;
using TkRenderer.Shapes;

namespace TkRenderer.Math
{
    internal static class FloatMath
    {
        public const float Pi = (float)System.Math.PI;

        public static float Sin(float x) => (float)System.Math.Sin(x);
        public static float Cos(float x) => (float)System.Math.Cos(x);
        public static float Atan2(float s, float c) => (float)System.Math.Atan2(s, c);
        public static float Pow(float a, float b) => (float)System.Math.Pow(a, b);
        public static float Pow(float a, int b) => (float)System.Math.Pow(a, b);
        public static float Sqrt(float x) => (float)System.Math.Sqrt(x);
        public static float Exp(float x) => (float)System.Math.Exp(x);
        public static float Abs(float x) => System.Math.Abs(x);
        public static float Min(float a, float b) => a > b ? a : b;     
        public static float Max(float a, float b) => a < b ? a : b;
        public static int   Min(int a, int b) => a > b ? a : b;
        public static int   Max(int a, int b) => a < b ? a : b;
        public static float Fabs(float f) => f < 0 ? -f : f;
        public static float Clamp(float value, float min, float max) => value < min ? min : value > max ? max : value;
        public static float Clamp01(float value) => Clamp(value, 0, 1);

        public static readonly Random Random = new Random();

        public static float Rand01 => (float)Random.NextDouble();
        public static float Rand(float max) => Rand01 * max;
        public static float Rand(float min, float max) => min + Rand01 * (max - min);

        public static Vector2 RandomUnitVector => Vector2.FromAngle(RandomAngle);
        public static float RandomAngle => Rand(2.0f * Pi);

        public static void Swap<T>(ref T a, ref T b)
        {
            var tmp = a;
            a = b;
            b = tmp;
        }

        public static float Min(float[] a)
        {
            if (a == null)
            {
                throw new NullReferenceException(nameof(a));
            }
            Debug.Assert(a.Length != 0);
            var min = a[0];
            for ( var i = 1; i < a.Length; i++ )
            {
                if (a[i] < min)
                    min = a[i];
            }
            return min;
        }


        public static float Max(float[] a)
        {
            if (a == null)
            {
                throw new NullReferenceException(nameof(a));
            }
            Debug.Assert(a.Length != 0);
            var max = a[0];
            for (var i = 1; i < a.Length; i++)
            {
                if (a[i] > max)
                    max = a[i];
            }
            return max;
        }


        public static class Clamper
        {
            public static Vector2 ClampCircleInsideRect(Vector2 cPos, float r, Rect rect) =>
                new Vector2(Clamp(cPos.X, rect.Left + r, rect.Right - r), Clamp(cPos.Y, rect.Bottom + r, rect.Top - r));   
        }
    }
}
