using TkRenderer.Math.LinearAlgebra;
using TkRenderer.Shapes;

namespace TkRenderer.Drawing
{
    public class Visual
    {
        public Visual(Vector2[] positions, int[] indices, Color4 color, float z, bool useSoftwareTransform, Rect boundingRect)
        {
            Positions = positions;
            Indices = indices;
            Color = color;
            Z = z;
            UseTransform = useSoftwareTransform;
            BoundingRect = boundingRect;
        }
 
        public Vector2[] Positions;
        public int[] Indices;
        public float Z;
        public Color4 Color;
        public bool UseTransform;
        public Affine Transform = Affine.Identity;
        public readonly Rect BoundingRect;
        public Rect WorldBoundingRect;
    }
}

