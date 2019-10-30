using TkRenderer.Math.LinearAlgebra;

namespace TkRenderer.Shapes
{
    public struct Rect : IShape
    {
        private static (float mid, float length) EndpointsToCentered(float min, float max) => ((min + max) / 2.0f, max - min);
        private static (float min, float max) CenteredToEndpoints(float mid, float length) => (mid - length/2.0f, mid + length/2.0f);



        public Rect(Vector2 center, float width, float height)
        {
            (Left, Right) = CenteredToEndpoints(center.X, width);
            (Bottom, Top) = CenteredToEndpoints(center.Y, height);
        }

        public Rect(float left, float right, float bottom, float top)
        {
            Left = left;
            Right = right;
            Bottom = bottom;
            Top = top;
        }
        
        public Vector2 Center
        {
            get => new Vector2((Left + Right)/2.0f, (Top + Bottom)/2.0f);
            set
            {
                (Left, Right) = CenteredToEndpoints(value.X, Width);
                (Bottom, Top) = CenteredToEndpoints(value.Y, Height);
            }
        }

        public float Width
        {
            get => Right - Left;
            set => this = new Rect(Center, value, Height);
        }

        public float Height
        {
            get => Top - Bottom;
            set => this = new Rect(Center, Width, value);
        }

        public void Translate(Vector2 translation)
        {
            Center += translation;
        }


        public Rect Translated(Vector2 translation)
        {
            return new Rect(
                Left + translation.X,
                Right + translation.X,
                Bottom + translation.Y,
                Top + translation.Y);
        }

        public float Left;
        public float Right;
        public float Bottom;
        public float Top;

        public Vector2 BottomLeft => new Vector2(Left, Bottom);
        public Vector2 BottomRight => new Vector2(Right, Bottom);
        public Vector2 TopLeft => new Vector2(Left, Top);
        public Vector2 TopRight => new Vector2(Right, Top);
        public Vector2[] Vertices => new [] { BottomLeft, TopLeft, TopRight, BottomRight };

        public Vector2 Centroid => Center;

        public float Area => Width * Height;

        public float Perimeter => 2.0f * Width + 2.0f * Height;

        public static bool IntersectRects(Rect a, Rect b)
            => !(a.Right < b.Left || a.Left > b.Right || a.Bottom > b.Top || a.Top < b.Bottom);

        public static bool RectContainsRect(Rect container, Rect contained)
            => container.Left < contained.Left && container.Right > contained.Right && container.Top > contained.Top && container.Bottom < contained.Bottom;
    }
}
