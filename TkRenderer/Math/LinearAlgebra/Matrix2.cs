using static TkRenderer.Math.FloatMath;
using static TkRenderer.Math.LinearAlgebra.Vector2;

namespace TkRenderer.Math.LinearAlgebra
{
    public struct Matrix2
    {
        public static Matrix2 Identity => new Matrix2(1, 0, 0, 1);
        public static Matrix2 Zero => new Matrix2(0, 0, 0, 0);

        public float A00, A01, A10, A11;

        public Vector2 Row0
        {
            get => (A00, A01);
            set => (A00, A01) = value;
        }

        public Vector2 Row1
        {
            get => (A10, A11);
            set => (A10, A11) = value;
        }

        public Vector2 Col0
        {
            get => (A00, A10);
            set => (A00, A10) = value;
        }
        public Vector2 Col1
        {
            get => (A01, A11);
            set => (A01, A11) = value;
        }

        public Matrix2(float e00, float e01, float e10, float e11) => (A00, A01, A10, A11) = (e00, e01, e10, e11);

        public static Matrix2 Rotation(float angle) => new Matrix2(Cos(angle), -Sin(angle), Sin(angle), Cos(angle));
        public static Matrix2 Scale(float s) => new Matrix2(s, 0, 0, s);
        public static Matrix2 Scale(float sx, float sy) => new Matrix2(sx, 0, 0, sy);
        public static Matrix2 ReflectionX => new Matrix2(-1, 0, 0, 1);
        public static Matrix2 ReflectionY => new Matrix2(1, 0, 0, -1);
        public static Matrix2 Transpose(Matrix2 m) => new Matrix2(m.A00, m.A10, m.A01, m.A11);
        public void Transpose() => this = new Matrix2(A00, A10, A01, A11);
        public static float Det(Matrix2 m) => m.A00 * m.A11 - m.A10 * m.A11;
        
        public static Matrix2 operator * (Matrix2 l, Matrix2 r) => new Matrix2
            (Dot(l.Row0, r.Col0), Dot(l.Row0, r.Col1), 
             Dot(l.Row1, r.Col0), Dot(l.Row1, r.Col1));
        public static Vector2 operator * (Matrix2 m, Vector2 v) => new Vector2(Dot(m.Row0, v), Dot(m.Row1, v));
        public static Matrix2 operator * (float f, Matrix2 m) => new Matrix2(f * m.A00, f * m.A01, f * m.A10, f * m.A11);

        public static explicit operator float[] (Matrix2 m) => new [] { m.Row0.X, m.Row0.Y, m.Row1.X, m.Row1.Y };
        public static implicit operator Matrix2((float, float, float, float) m) => new Matrix2(m.Item1, m.Item2, m.Item3, m.Item4);

    }
}