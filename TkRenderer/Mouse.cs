using OpenTK.Input;
using TkRenderer.Math.LinearAlgebra;

namespace TkRenderer
{
    public struct Mouse
    {
        public Mouse(int windowPosX, int windowPosY, int windowDeltaX, int windowDeltaY, Affine windowToView, Affine windowToWorld)
        {
            WindowPosX = windowPosX;
            WindowPosY = windowPosY;
            WindowDeltaX = windowDeltaX;
            WindowDeltaY = windowDeltaY;
            var wPos = new Vector2(windowPosX, windowPosY);
            var wDelta = new Vector2(windowDeltaX, windowDeltaY);
            ViewPos = windowToView * wPos;
            ViewDelta = windowToView.A * wDelta;
            WorldPos = windowToWorld * wPos;
            WorldDelta = windowToWorld.A * wDelta;
        }

        public int WindowPosX { get; }
        public int WindowPosY { get; }
        public int WindowDeltaX { get; }
        public int WindowDeltaY { get; }
        public Vector2 ViewPos { get; }
        public Vector2 ViewDelta { get; }
        public Vector2 WorldPos { get; }
        public Vector2 WorldDelta { get; }

        public bool LeftButtonDown => OpenTK.Input.Mouse.GetState().IsButtonDown(MouseButton.Left);
        public bool RightButtonDown => OpenTK.Input.Mouse.GetState().IsButtonDown(MouseButton.Right);
        public bool MiddleButtonDown => OpenTK.Input.Mouse.GetState().IsButtonDown(MouseButton.Middle);
        public float WheelValue => OpenTK.Input.Mouse.GetState().WheelPrecise;
    }
}