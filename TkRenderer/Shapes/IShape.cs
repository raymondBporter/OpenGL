using TkRenderer.Math.LinearAlgebra;

namespace TkRenderer.Shapes
{
    interface IShape
    {
        Vector2 Centroid { get; }
        float Area { get; }
        float Perimeter { get; }
    }
}
