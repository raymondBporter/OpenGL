using TkRenderer.Math.LinearAlgebra;
using static TkRenderer.Math.FloatMath;

namespace TkRenderer.Shapes
{
    public struct Circle : IShape
    {
        public Vector2 Center;
        public float Radius;

        public Vector2 Centroid => Center;

        public float Area => Pi * Radius * Radius;

        public float Perimeter => 2.0f * Pi * Radius;
    }
}
