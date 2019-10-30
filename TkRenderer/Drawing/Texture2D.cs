using OpenTK.Graphics.OpenGL4;

namespace TkRenderer.Drawing
{
    public class Texture2D : GlObject
    {
        public Texture2D(int width, int height) : base(GL.GenTexture(), GlType.Texture)
        {
            Width = width;
            Height = height;
        }

        public void Bind() => GL.BindTexture(TextureTarget.Texture2D, Handle);
        public static void UnBind() => GL.BindTexture(TextureTarget.Texture2D, 0);

        public readonly int Width;
        public readonly int Height;
    }
}
