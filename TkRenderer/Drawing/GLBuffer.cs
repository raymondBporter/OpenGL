using System;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;

namespace TkRenderer.Drawing
{
    public class GlBuffer : GlObject
    {
        protected GlBuffer(BufferTarget target) : base(GL.GenBuffer(), GlType.Buffer) => Target = target;
        public void SetData<T>(T[] data, BufferUsageHint usageHint) where T : struct =>
            GL.BufferData(Target, (IntPtr)(Marshal.SizeOf(typeof(T)) * data.Length), data, usageHint);
        public void Bind() => GL.BindBuffer(Target, Handle);
        protected BufferTarget Target;
    }

    public class IndexBuffer : GlBuffer
    {
        public IndexBuffer() : base(BufferTarget.ElementArrayBuffer) { }
        public static void UnBind() => GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
    }

    public class VertexBuffer : GlBuffer
    {
        public VertexBuffer() : base(BufferTarget.ArrayBuffer) { }
        public static void UnBind() => GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
    }
}