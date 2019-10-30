using OpenTK.Graphics.ES20;
using OpenTK.Input;
using System;
using System.Drawing;
using TkRenderer.Drawing;
using TkRenderer.Programs;

namespace TkRenderer
{
    public static class KeyBoard
    {
        public static bool IsDown(Key key) => Keyboard.GetState()[key];
    }

    public class BaseProgram
    {
        public BaseProgram(int width, int height) 
        {
            _camera = new Camera(width, height, 90.0f);
            _world = new DrawShapesWorld();
        }

        public void UpdateFrame()
        {
            Tick();

            _camera.HandleInput(_timer, _mouse);
            _world.HandleInput(_mouse, _timer);
            _world.Update(_timer);
        }

        public void Load()
        {           
        }

        public void UnLoad()
        {
        }

        private void Tick()
        {
            if (_timer is null)
            {
                _timer = new Timer();
                _timer.Start();
            }
            _timer.Tick();
        }

        public void MouseMove(int windowPosX, int windowPosY, int windowDeltaX, int windowDeltaY)
        {
            _mouse = new Mouse(windowPosX, windowPosY, windowDeltaX, windowDeltaY, _camera.WindowToView, _camera.WindowToWorld);
        }

        public void Resize(int width, int height)
        {
            if (height == 0)
                height = 1;
            GL.Viewport(new Size(width, height));
            _camera.WindowWidth = width;
            _camera.WindowHeight = height;
        }

        public void RenderFrame(Action swapBuffers)
        {
            GL.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            _world.Draw(_camera, _timer);
            swapBuffers();
        }


        private Mouse _mouse;
        private Timer _timer = null;
        private readonly Camera _camera;
        private readonly DrawShapesWorld _world;
    }
}
