using OpenTK.Graphics.OpenGL4;

namespace TkRenderer.Drawing
{
    public class VertexArray : GlObject
    {
        public VertexArray() : base(GL.GenVertexArray(), GlType.VertexArray) { }
        public void Bind() => GL.BindVertexArray(Handle);
        public static void UnBind() => GL.BindVertexArray(0);
    }
}
