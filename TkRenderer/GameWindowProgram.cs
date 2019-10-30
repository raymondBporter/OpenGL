using System;
using OpenTK;
using OpenTK.Input;


namespace TkRenderer
{
    public class GameWindowProgram : GameWindow
    {
        public GameWindowProgram(int width, int height) : base(width, height)
        {
            _baseProgram = new BaseProgram(width, height);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            _baseProgram.UpdateFrame();
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            _baseProgram.MouseMove(e.X, e.Y, e.XDelta, e.YDelta);
        }

        protected override void OnLoad(EventArgs e)
        {
            _baseProgram.Load();
        }

        protected override void OnUnload(EventArgs e)
        {
            _baseProgram.UnLoad();
        }

        protected override void OnResize(EventArgs e)
        {
            _baseProgram.Resize(ClientSize.Width, ClientSize.Height);
        }

        protected override void OnRenderFrame(FrameEventArgs eventArgs)
        {
            _baseProgram.RenderFrame(SwapBuffers);
        }


        private readonly BaseProgram _baseProgram;


        //[STAThread]
        public static void Main()
        {
            using var program = new GameWindowProgram(1024, 786);
            program.Run();
        }
    }
    
}
