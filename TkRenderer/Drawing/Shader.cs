using System.Diagnostics;
using OpenTK.Graphics.OpenGL4;

namespace TkRenderer.Drawing
{
    public class Shader : GlObject
    {
        public Shader(ShaderType shaderType, string source) : base(GL.CreateShader(shaderType), GlType.Program)
        {
            GL.ShaderSource(Handle, source);
            Compile();
            Debug.Assert(IsCompiled(), GetErrorText());
        }

        private void Compile() => GL.CompileShader(Handle);

        public bool IsCompiled()
        {
            GL.GetShader(Handle, ShaderParameter.CompileStatus, out var isCompiled);
            return isCompiled != 0;
        }

        public string GetErrorText()
        {
            if (!IsCompiled())
                return "Shader is not compiled";

            GL.GetShader(Handle, ShaderParameter.InfoLogLength, out var maxLength);
            GL.GetShaderInfoLog(Handle, maxLength, out _, out var infoLog);
            return infoLog;

        }
    }
}
