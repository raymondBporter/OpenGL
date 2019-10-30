namespace TkRenderer.Math.LinearAlgebra
{
    public struct Affine
    {
        public Affine(Matrix2 mat, Vector2 vec) 
        {
            A = mat; 
            D = vec; 
        }
        public Affine(float scale, float rotAngle, Vector2 translation) : this (scale * Matrix2.Rotation(rotAngle), translation) { }
        public Affine(float rotAngle, Vector2 translation) : this(Matrix2.Rotation(rotAngle), translation) { }  

        public static explicit operator Matrix3(Affine x) => new Matrix3
            (x.A.A00, x.A.A01, x.D.X, 
             x.A.A10, x.A.A11, x.D.Y, 
             0.0f,       0.0f,  1.0f);
        public static Vector2 operator *(Affine a, Vector2 x) => 
            a.A * x + a.D;
        public static Affine operator *(Affine a, Affine b) => new Affine
            (a.A * b.A, a.A * b.D + a.D);

        public static Affine Identity = new Affine
            (Matrix2.Identity, Vector2.Zero );    
        public static Affine Rotation(float r) => new Affine
            (Matrix2.Rotation(r), Vector2.Zero);    
        public static Affine Translation(Vector2 d) => new Affine
            (Matrix2.Identity, d);
        public static Affine Scale(float s) => new Affine(Matrix2.Scale(s), Vector2.Zero);
        public static Affine Scale(float sx, float sy) => new Affine(Matrix2.Scale(sx, sy), Vector2.Zero);
        public static Affine ReflectionX => new Affine(Matrix2.ReflectionX, Vector2.Zero);
        public static Affine ReflectionY => new Affine(Matrix2.ReflectionY, Vector2.Zero);

        public Matrix2 A;
        public Vector2 D;
    }
}
