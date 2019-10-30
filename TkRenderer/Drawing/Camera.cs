using OpenTK.Input;
using TkRenderer.Math.LinearAlgebra;
using TkRenderer.Shapes;
using static TkRenderer.Math.LinearAlgebra.Affine;
using static TkRenderer.Math.FloatMath;

namespace TkRenderer.Drawing
{
    public class Camera
    {
        public Camera(float windowWidth, float windowHeight, float pixelsPerUnit)
        {
            PixelsPerUnit = pixelsPerUnit;
            WindowWidth = windowWidth;
            WindowHeight = windowHeight;
        }

        public Vector2 Position = Vector2.Zero;
        public float Angle;
        public float UnitsPerPixel;
        public float PixelsPerUnit
        {
            get => 1.0f / UnitsPerPixel;
            set => UnitsPerPixel = 1.0f / value;
        }
        public float WindowWidth;
        public float WindowHeight;
        public Vector2 WindowSize => (WindowWidth, WindowHeight);
        public float ViewWidth => WindowWidth * UnitsPerPixel;
        public float ViewHeight => WindowHeight * UnitsPerPixel;
        public Vector2 ViewSize => (ViewWidth, ViewHeight);
        public Affine WorldToView => Rotation(-Angle) * Translation(-Position);
        public Affine WindowToWorld => ViewToWorld * WindowToView;
        public Affine ViewToWorld => new Affine(Angle, Position);
        public Affine ViewToDevice => Scale(2.0f / ViewWidth, 2.0f / ViewHeight);
        public Affine WorldToDevice => ViewToDevice * WorldToView;
        public Affine WindowToView => Scale(UnitsPerPixel) * ReflectionY * Translation(-WindowSize / 2.0f);

        public Rect WorldSpaceBoundingRect { get; private set; }

        public void UpdateBoundingRect()
        {
            var viewRectVerts = new Rect(Vector2.Zero, ViewWidth, ViewHeight).Vertices;

            var v = ViewToWorld * viewRectVerts[0];

            var xMin = v.X;
            var yMin = v.Y;
            var xMax = v.X;
            var yMax = v.Y;

            for (var i = 1; i < 4; i++)
            {
                v = ViewToWorld * viewRectVerts[i];
                if (v.X < xMin)
                {
                    xMin = v.X;
                }
                if (v.X > xMax)
                {
                    xMax = v.X;
                }
                if (v.Y < yMin)
                {
                    yMin = v.Y;
                }
                if (v.Y > yMax)
                {
                    yMax = v.Y;
                }
            }
            WorldSpaceBoundingRect = new Rect(xMin, xMax, yMin, yMax);
        }


        public void HandleInput(Timer timer, Mouse mouse)
        {

            double dt = timer.DeltaTime;

            var v = Vector2.Zero;

            if (KeyPressed(Key.Up))
            {
                v += Vector2.YAxis;
            }
            if (KeyPressed(Key.Down))
            {
                v -= Vector2.YAxis;
            }
            if (KeyPressed(Key.Left))
            {
                v -= Vector2.XAxis;
            }
            if (KeyPressed(Key.Right))
            {
                v += Vector2.XAxis;
            }
            if (KeyPressed(Key.A))
            {
                Angle += AngularSpeed * (float)dt;
            }
            if (KeyPressed(Key.S))
            {
                Angle -= AngularSpeed * (float)dt;
            }
            if (KeyPressed(Key.Z))
            {
                UnitsPerPixel *= Exp(ZoomSpeed * (float)dt);
            }
            if (KeyPressed(Key.X))
            {
                UnitsPerPixel *= Exp(-ZoomSpeed * (float)dt);
            }
            if (v != Vector2.Zero)
            {
                v.Normalize();
            }

            UnitsPerPixel = mouse.WheelValue;
            Position += new Rotation2(Angle) * ( LinearSpeed * UnitsPerPixel * (float)dt * v );
            UpdateBoundingRect();
        }

        private const float ZoomSpeed = 1.02f;
        private const float AngularSpeed = 3f;
        private const float LinearSpeed = 250.0f;

        private static bool KeyPressed(Key key) => Keyboard.GetState()[key];
    }
}