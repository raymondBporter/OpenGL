using System.Runtime.InteropServices;
using static TkRenderer.Math.FloatMath;

namespace TkRenderer.Math.LinearAlgebra
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector2
    {
        public Vector2(float x, float y) => (X, Y) = (x, y);

        public static Vector2 Zero => new Vector2(0, 0);
        public static Vector2 XAxis => new Vector2(1, 0);
        public static Vector2 YAxis => new Vector2(0, 1);

        public static Vector2 FromAngle(float a) => new Vector2(Cos(a), Sin(a));
        public static float Dot(Vector2 u, Vector2 v) => u.X * v.X + u.Y * v.Y;
        public static float Cross(Vector2 u, Vector2 v) => u.X * v.Y - u.Y * v.X;
        public static float Distance(Vector2 u, Vector2 v) => (u - v).Length;
        public static float DistanceSq(Vector2 u, Vector2 v) => (u - v).LengthSq;
        public float Length => Sqrt(X * X + Y * Y);
        public float LengthSq => X * X + Y * Y;
        public Vector2 Normalized => this / Length;
        public void Normalize() => this /= Length;
        public Vector2 Clamp(float len) => this = Clamped(len);
        public Vector2 Clamped(float len) => Dot(this, this) > len * len ? Normalized * len : this;
        public Vector2 PerpLeft => new Vector2(-Y, X);
        public Vector2 PerpRight => new Vector2(Y, -X);
        public static Vector2 Lerp(Vector2 v1, Vector2 v2, float t) => (1 - t) * v1 + t * v2;
        public static Vector2 operator -(Vector2 v) => new Vector2(-v.X, -v.Y);
        public static bool operator ==(Vector2 u, Vector2 v) => u.X == v.X && u.Y == v.Y;
        public static bool operator !=(Vector2 u, Vector2 v) => !(u == v);
        public static Vector2 operator +(Vector2 u, Vector2 v) => new Vector2(u.X + v.X, u.Y + v.Y);
        public static Vector2 operator -(Vector2 u, Vector2 v) => new Vector2(u.X - v.X, u.Y - v.Y);
        public static Vector2 operator *(float s, Vector2 v) => new Vector2(s * v.X, s * v.Y);
        public static Vector2 operator *(Vector2 v, float s) => new Vector2(s * v.X, s * v.Y);
        public static Vector2 operator /(Vector2 v, float s) => new Vector2(v.X / s, v.Y / s);




        // public bool Length => Sqrt(X * X + Y * Y); 

        public override bool Equals(object obj) => obj is Vector2 v && this == v;
        public override int GetHashCode() => HashCodeHelper.GetHashCode(X, Y);
        public static explicit operator float[](Vector2 v) => new [] { v.X, v.Y };
        public static implicit operator Vector2((float, float) v) =>  new Vector2(v.Item1, v.Item2);
        public void Deconstruct(out float x, out float y) { x = X; y = Y; }

        public float X;
        public float Y;
    }
}
