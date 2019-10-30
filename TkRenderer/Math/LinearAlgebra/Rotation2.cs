using static TkRenderer.Math.FloatMath;


namespace TkRenderer.Math.LinearAlgebra
{
    public struct Rotation2
    {
        public Rotation2(float cos, float sin) { _c = cos; _s = sin; }
        public Rotation2(float angle) : this(Cos(angle), Sin(angle)) {}    
        public static Rotation2 Identity => new Rotation2(1.0f, 0.0f);
        public static Vector2 operator * (Rotation2 r, Vector2 v) => (v.X * r._c - v.Y * r._s, v.X * r._s + v.Y * r._c);
        public static Rotation2 operator * (Rotation2 a, Rotation2 b) => new Rotation2(a._c * b._c - a._s * b._s, a._s * b._c + a._c * b._s);
        public static Rotation2 operator - (Rotation2 r) => new Rotation2(-r._c, -r._s);
        public float Angle() => Atan2(_s, _c);
        public Rotation2 Inverse => new Rotation2(-_s, _c);

        private readonly float _c, _s;
    }
}
