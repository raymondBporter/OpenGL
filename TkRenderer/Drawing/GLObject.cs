using System;
using OpenTK.Graphics.OpenGL4;

namespace TkRenderer.Drawing
{
    public class GlObject : IDisposable
    {
        public enum GlType { Buffer, VertexArray, Program, Texture }

        public GlObject(int handle, GlType glType)
        {
            Handle = handle;
            _glType = glType;
        }

        public int Handle { get; protected set; }
        private readonly GlType _glType;

        private bool _disposed; // To detect redundant calls
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }
            switch (_glType)
            {
                case GlType.Buffer:      GL.DeleteBuffer(Handle);      break;
                case GlType.Program:     GL.DeleteProgram(Handle);     break;
                case GlType.VertexArray: GL.DeleteVertexArray(Handle); break;
                case GlType.Texture:     GL.DeleteTexture(Handle);     break;
                default: throw new NotImplementedException();
            }
            _disposed = true;
        }
    }
    
}
