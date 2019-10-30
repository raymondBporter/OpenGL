using TkRenderer.Math.LinearAlgebra;
using static TkRenderer.Math.FloatMath;

namespace TkRenderer.Shapes
{
    public class Capsule : IShape
    {
        public Vector2 A;
        public Vector2 B;
        public float Radius;
        public float UncappedLength => (B - A).Length;
        public Vector2 Centroid => ( A + B ) / 2.0f;

        public float Area => UncappedLength * Radius + Pi * Radius * Radius;

        public float Perimeter => 2.0f * Pi * Radius + 2.0f * UncappedLength;
    }
}
