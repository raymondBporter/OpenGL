using System.Collections.Generic;
using System.Linq;
using TkRenderer.Drawing;
using TkRenderer.Math.LinearAlgebra;
using TkRenderer.Shapes;
using static TkRenderer.Math.FloatMath;

namespace TkRenderer.Programs
{
    internal class TestWorld
    {
        private const int NumVisuals = 20;
        private readonly ConvexPolygon[] _polygons = new ConvexPolygon[NumVisuals];
        private readonly Body[] _bodies = new Body[NumVisuals];
        private readonly PolyLine[] _polyLines = new PolyLine[2];


        private struct Body
        {
            public Vector2 Position;
            public Vector2 Velocity;
            public float Angle;
            public float AngleVelocity;
            public Affine Transform => new Affine(Angle, Position);
        }

        private static IEnumerable<float> IntervalStep(float begin, float end, float step)
        {
            var n = (end - begin) / step;

            for (var i = 0; i < n; i++)
            {
                yield return begin + i * step;
            }
        }

        public TestWorld()
        {
            _scene = new Batch();



            _polyLines[0] = new PolyLine(
                          IntervalStep(1, 10, 0.1f)
                          .Select(j => j * Vector2.FromAngle(j))
                          .ToArray())
            {
                Thickness = 0.7f
            };


            _polyLines[1] = new PolyLine(
                IntervalStep(1, 60, 0.05f)
                .Select(j => new Vector2(
                    j / 10.0f,
                    Cos(j/10)
                    ))
                .ToArray())
            {
                Thickness = 0.8f
            };



            for ( var i = 0; i < NumVisuals; i++ )
            {
                _polygons[i] = new ConvexPolygon(VisualFactory.CircleVertices(7.0f, (int)Rand(6) + 3), Rand01 * 3.0f);
                _bodies[i] = new Body
                {
                    Position = (Rand(-20, 20), Rand(-20, 20)),
                    Velocity = Rand(3.0f) * RandomUnitVector,
                    Angle = 0, //RandomAngle;
                    AngleVelocity = 0 // 1.5f + Rand01;
                };
                _colors[i] = new Color4(Rand(1.0f), Rand(1.0f), Rand(1.0f), 1.0f);
                _color2[i] = new Color4(Rand(1.0f), Rand(1.0f), Rand(1.0f), 1.0f);
            }
            
        }

        public void HandleInput(Vector2 mouseWorldPos, Vector2 mouseDelta, double deltaTime)
        {

        }

        public void Update(double deltaTime)
        {
            for (var i = 0; i < NumVisuals; i++)
            {
                _bodies[i].Position += _bodies[i].Velocity * (float)deltaTime;
                _bodies[i].Angle += _bodies[i].AngleVelocity * (float)deltaTime;
            }
        }

        public void Draw(Camera camera, double time, double deltaTime)
        {
            const float end = 100f;
            const float w = 6 * 2.0f * Pi / end;
            const float r = 14.0f;
            //_scene.DrawPolyLine(IntervalStep(0, 10, 0.1).Select(x => r * Sin(w*x)), _colors[0], 0, end, 0.1f);
            _scene.Draw(camera.WorldToDevice);
        }

        private readonly Color4[] _colors = new Color4[NumVisuals];
        private readonly Color4[] _color2 = new Color4[NumVisuals];

        private readonly Batch _scene;
    }

}

