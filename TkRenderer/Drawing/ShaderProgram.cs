using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;

namespace TkRenderer.Drawing
{
    public class ShaderProgram : GlObject
    {
        public ShaderProgram(Shader vertexShader, Shader fragmentShader) : base(GL.CreateProgram(), GlType.Program)
        {
            VertexShader = vertexShader;
            FragmentShader = fragmentShader;
            GL.AttachShader(Handle, VertexShader.Handle);
            GL.AttachShader(Handle, FragmentShader.Handle);
            GL.LinkProgram(Handle);
        }

        private int GetUniformLocation(string name)
        {
            if (UniformLocations.TryGetValue(name, out var location)) return location;
            location = GL.GetUniformLocation(Handle, name);
            UniformLocations.Add(name, location);
            return location;
        }

        public void SetMatrix3(string name, float[] matrix) => GL.UniformMatrix3(GetUniformLocation(name), 1, true, matrix);
        public void SetInt(string name, int value) => GL.Uniform1(GetUniformLocation(name), value);

        public void Begin() => GL.UseProgram(Handle);
        public static void End() => GL.UseProgram(0);

        protected Shader VertexShader;
        protected Shader FragmentShader;
        public Dictionary<string, int> UniformLocations = new Dictionary<string, int>();


        public static readonly ShaderProgram Colored = new ShaderProgram(
                    ResourceManager.GetVertexShader(@"ColoredTransformed.vsh"),
                    ResourceManager.GetFragmentShader(@"Colored.psh"));
    }
}
