using OpenTK.Graphics.OpenGL4;

namespace TkRenderer.Drawing
{
    public class VertexAttribute
    {
        private readonly int _index;
        private readonly int _size;
        private readonly int _stride;
        private readonly int _offset;
        private readonly VertexAttribPointerType _attributeType;

        public VertexAttribute(int index, int size, VertexAttribPointerType attributeType, int stride, int offset)
        {
            _size = size;
            _index = index;
            _attributeType = attributeType;
            _stride = stride;
            _offset = offset;
        }

        public void SetAttributePointer()
        {
            GL.EnableVertexAttribArray(_index);
            GL.VertexAttribPointer(_index, _size, _attributeType, false, _stride, _offset);
        }
    }
}

