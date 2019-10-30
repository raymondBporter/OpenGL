using System;
using TkRenderer.Math.LinearAlgebra;

namespace TkRenderer.Shapes
{
    public class Triangle : IShape
    {
        public Vector2 V0;
        public Vector2 V1;
        public Vector2 V2;

        public Vector2 this[int i]
        { 
            get => i switch
                {
                    0 => V0,
                    1 => V1,
                    2 => V2,
                    _ => throw new ArgumentOutOfRangeException()
                };
            set
            {
                switch (i)
                {
                    case 0: V0 = value; break;
                    case 1: V1 = value; break;
                    case 2: V2 = value; break;
                    default : throw new ArgumentOutOfRangeException();
                }
            }
        }

        public Vector2 Centroid => (V0 + V1 + V2) / 3.0f;

        public float Area => 0.5f * Vector2.Dot(V1 - V0, (V2 - V0).PerpLeft) ;

        public float Perimeter => (V0 - V1).Length + (V1 - V2).Length + (V2 - V0).Length;
    }
}
