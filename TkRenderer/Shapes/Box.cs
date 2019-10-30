using TkRenderer.Math.LinearAlgebra;

namespace TkRenderer.Shapes
{
    public class Box : IShape
    {
        public Vector2 Center;

        public Vector2 Size;

        public Vector2 Centroid => Center;

        public float Area => Size.X * Size.Y;

        public float Perimeter => 2.0f * Size.X + 2.0f * Size.Y;

    }
}
