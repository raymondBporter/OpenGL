using System.Runtime.InteropServices;
using static TkRenderer.Math.FloatMath;

namespace TkRenderer.Math.LinearAlgebra
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3
    {
        public Vector3(float x, float y, float z) => (X, Y, Z) = (x, y, z);
        public Vector3(Vector2 xy, float z) => (X, Y, Z) = (xy.X, xy.Y, z);

        public static readonly Vector3 Zero = new Vector3(0, 0, 0);
        public static readonly Vector3 XAxis = new Vector3(1, 0, 0);
        public static readonly Vector3 YAxis = new Vector3(0, 1, 0);
        public static readonly Vector3 ZAxis = new Vector3(0, 0, 1);

        public void Normalize() => this /= Length;
        public Vector3 Normalized => this / Length;
        public static float Dot(Vector3 u, Vector3 v) => u.X * v.X + u.Y * v.Y + u.Z * v.Z;
        public float Length => Sqrt(X * X + Y * Y + Z * Z);
        public float LengthSq => X * X + Y * Y + Z * Z;
        public static Vector3 operator -(Vector3 v) => new Vector3(-v.X, -v.Y, -v.Z);
        public static Vector3 operator +(Vector3 u, Vector3 v) => new Vector3(u.X + v.X, u.Y + v.Y, u.Z + v.Z);
        public static Vector3 operator -(Vector3 u, Vector3 v) => new Vector3(u.X - v.X, u.Y - v.Y, u.Z - v.Z);
        public static Vector3 operator *(float s, Vector3 v) => new Vector3(s * v.X, s * v.Y, s * v.Z);
        public static Vector3 operator *(Vector3 v, float s) => new Vector3(s * v.X, s * v.Y, s * v.Z);
        public static Vector3 operator /(Vector3 v, float s) => new Vector3(v.X / s, v.Y / s, v.Z / s);
        public Vector2 Xy => new Vector2(X, Y);

        public static implicit operator Vector3((float, float, float) v) => new Vector3(v.Item1, v.Item2, v.Item3);
        public static explicit operator float[] (Vector3 v) => new [] { v.X, v.Y, v.Z };
        public void Deconstruct(out float x, out float y, out float z) { x = X; y = Y; z = Z; }

        public float X;
        public float Y;
        public float Z;
    }
}
