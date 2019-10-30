using static TkRenderer.Math.FloatMath;
using static TkRenderer.Math.LinearAlgebra.Vector3;

namespace TkRenderer.Math.LinearAlgebra
{
    public struct Matrix3 
    {
        public static readonly Matrix3 Identity = new Matrix3
            ( 1, 0, 0, 
              0, 1, 0, 
              0, 0, 1 );

        public static readonly Matrix3 Zero = new Matrix3
            ( Vector3.Zero, 
              Vector3.Zero, 
              Vector3.Zero );

        public Vector3 Row0;
        public Vector3 Row1;
        public Vector3 Row2;

        public Matrix3(Vector3 row0, Vector3 row1, Vector3 row2)
        {
            Row0 = row0;
            Row1 = row1;
            Row2 = row2;
        }

        public Matrix3(float m00, float m01, float m02,
                       float m10, float m11, float m12,
                       float m20, float m21, float m22) : this
            ( new Vector3(m00, m01, m02), 
              new Vector3(m10, m11, m12), 
              new Vector3(m20, m21, m22) ) { }


        public Vector3 Col0 => new Vector3(Row0.X, Row1.X, Row2.X);
        public Vector3 Col1 => new Vector3(Row0.Y, Row1.Y, Row2.Y); 
        public Vector3 Col2 => new Vector3(Row0.Z, Row1.Z, Row2.Z);

        public static Matrix3 RotationZ(float angle) => new Matrix3
            ( Cos(angle), -Sin(angle), 0, 
              Sin(angle),  Cos(angle), 0,
              0,           0,          1 );

        public static Matrix3 Scale(float s) => new Matrix3
            ( s, 0, 0,
              0, s, 0, 
              0, 0, s );
        public static Matrix3 Scale(float sx, float sy, float sz) => new Matrix3
            ( sx, 0,  0, 
              0,  sy, 0, 
              0,  0,  sz );
        public static Matrix3 Transpose(Matrix3 m) => new Matrix3(m.Col0, m.Col1, m.Col2);
     
        public static Matrix3 operator *(Matrix3 l, Matrix3 r) => new Matrix3
            ( Dot(l.Row0, r.Col0), Dot(l.Row0, r.Col1), Dot(l.Row0, r.Col2), 
              Dot(l.Row1, r.Col0), Dot(l.Row1, r.Col1), Dot(l.Row1, r.Col2), 
              Dot(l.Row2, r.Col0), Dot(l.Row2, r.Col1), Dot(l.Row2, r.Col2) );

        public static explicit operator float[] (Matrix3 m) => new []
        {
              m.Row0.X, m.Row0.Y, m.Row0.Z,
              m.Row1.X, m.Row1.Y, m.Row1.Z,
              m.Row2.X, m.Row2.Y, m.Row2.Z
        };
        
    }
}