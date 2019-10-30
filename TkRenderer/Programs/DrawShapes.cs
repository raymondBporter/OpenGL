using OpenTK.Input;
using TkRenderer.Drawing;
using TkRenderer.Math.LinearAlgebra;

namespace TkRenderer.Programs
{
    public class DrawShapesWorld
    {
        private float _angle;
        private float _distance;

        private Vector2 l0, l1;
        private Vector2 p0, p1;


        private void MakeLine()
        {
            var d = Vector2.FromAngle(_angle);
            var n = d.PerpLeft;


            l0 = _distance * n + 5.0f * d;
            l1 = _distance * n - 5.0f * d;


            d = Vector2.FromAngle((float)System.Math.PI / 4.0f);
            n = d.PerpLeft;

            p0 = 2.0f * n + 5.0f * d;
            p1 = 2.0f * n - 5.0f * d;
        }

        public DrawShapesWorld()
        {
            _scene = new Batch();
        }
        private static bool KeyPressed(Key key) => Keyboard.GetState()[key];

        public void HandleInput(Mouse mouse, Timer timer)
        {
            if ( KeyPressed(Key.K) )
            {
                _angle += 0.01f;
            }
            if (KeyPressed(Key.L))
            {
                _angle -= 0.01f;
            }
            if (KeyPressed(Key.O))
            {
                _distance -= 0.01f;
            }
            if ( KeyPressed(Key.P))
            {
                _distance += 0.01f;
            }
            MakeLine();


            /*
                d0 = ( L1 - L0 ) / |L1 - L0|
                n0 = d0.PerpLeft
                
                d1 = ( P1 - P0 ) / |P1 - P0|
                n1 = d1.PerpLeft
              
                dot(n0, P0 + t d1) = d

                t = ( d - dot(n0, P0) ) / dot(n0, d1) 
              
              
             */

            var d0 = (l1 - l0).Normalized;
            var n0 = d0.PerpLeft;
            var d1 = (p1 - p0).Normalized;

            var t = (-_distance - Vector2.Dot(p0, n0)) / Vector2.Dot(n0, d1);

            _intersectionPoint = p0 + t * d1;
        }


        private Vector2 _intersectionPoint;
        public void Update(Timer timer)
        {

        }

        public void Draw(Camera camera, Timer timer)
        {

            _scene.DrawLine(l0, l1, 0.1f, Color4.Red, 0.05f);
            _scene.DrawLine(p0, p1, 0.1f, Color4.Blue, 0.05f);
            _scene.DrawCircle(_intersectionPoint, 0.2f, 0.1f, Color4.Cyan);
            _scene.Draw(camera.WorldToDevice);
        }


        private readonly Batch _scene;
    }

}

