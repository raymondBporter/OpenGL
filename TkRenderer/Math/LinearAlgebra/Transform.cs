namespace TkRenderer.Math.LinearAlgebra
{
    public struct Transform
    {
        public static Transform Identity => new Transform(0.0f, Vector2.Zero);
        public Transform(Rotation2 rot, Vector2 trans) { T = trans; R = rot; }
        public Transform(float rot, Vector2 trans) : this(new Rotation2(rot), trans) { }
        public static Vector2 operator * (Transform a, Vector2 x) => a.R * x + a.T;
        public static Transform operator * (Transform a, Transform b) => new Transform(a.R * b.R, a.R * b.T + a.T);
        public Transform Inverse => new Transform(R.Inverse, R.Inverse * -T);

        public Vector2 T;
        public Rotation2 R;
    }
}
