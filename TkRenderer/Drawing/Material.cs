namespace TkRenderer.Drawing
{
    public class Material 
    {
        public void Begin(float[] worldToDeviceFloats)
        {
            VertexDeclaration.SetAttributePointers();
            ShaderProgram.Begin();
            ShaderProgram.SetMatrix3("transform", worldToDeviceFloats);
        }

        public void End() => ShaderProgram.End();
        public VertexDeclaration VertexDeclaration { get; } = VertexDeclaration.ColoredVertex;

        private ShaderProgram ShaderProgram { get; } = ShaderProgram.Colored;
    }
}
