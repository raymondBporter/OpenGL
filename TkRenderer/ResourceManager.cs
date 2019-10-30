using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OpenTK.Graphics.OpenGL4;
using TkRenderer.Drawing;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace TkRenderer
{
    internal static class ResourceManager
    {
        public static Texture2D GetTexture(string filename) => GetResource(filename, Textures, LoadTexture);
        public static Shader GetFragmentShader(string filename) => GetResource(filename, Shaders, LoadFragmentShader);
        public static Shader GetVertexShader(string filename) => GetResource(filename, Shaders, LoadVertexShader);


        private static TResource GetResource<TResource>(string filename, IDictionary<string, TResource> loadedResources, Func<string, TResource> loadResourceFunc)
        {
            if (loadedResources.TryGetValue(filename, out var resource)) return resource;
            resource = loadResourceFunc(filename);
            loadedResources.Add(filename, resource);
            return resource;
        }

        private static Shader LoadFragmentShader(string filename) => LoadShader(ShaderType.FragmentShader, filename);
        private static Shader LoadVertexShader(string filename) => LoadShader(ShaderType.VertexShader, filename);
        private static Shader LoadShader(ShaderType shaderType, string filename) => new Shader(shaderType, GetTextFromFile(ShadersPath + filename));

        private static string GetTextFromFile(string filename)
        {
            try
            {
                using var sr = new StreamReader(filename);
                return sr.ReadToEnd();
            }
            catch (Exception e)
            {
                Debug.Assert(false, filename + " could not be read: " + e.Message);
                return "";
            }
        }

 
        public static Texture2D LoadTexture(string filename)
        {
            try
            {
                using var bitmap = new Bitmap(TexturesPath + filename);
                GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
                var texture = new Texture2D(bitmap.Width, bitmap.Height);
                texture.Bind();
                var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                var data = bitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                    OpenTK.Graphics.OpenGL4.PixelFormat.Rgba, PixelType.UnsignedByte, data.Scan0);
                bitmap.UnlockBits(data);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);      
                return texture;
            }
            catch
            {
                Debug.Assert(false, "file not found " + filename);
                return null;
            }
        }

        public static Texture2D GenerateTexture(int[] pixelArray, int width, int height)
        {
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
            var texture = new Texture2D(width, height);

            texture.Bind();
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0,
                OpenTK.Graphics.OpenGL4.PixelFormat.Rgba, PixelType.UnsignedByte, pixelArray);

            Console.WriteLine(GL.GetError().ToString());

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            return texture;
        }

        public static readonly Dictionary<string, Shader> Shaders = new Dictionary<string, Shader>();
        public static readonly Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();
        public const string TexturesPath = "../../Textures/";
        public const string ShadersPath = "../../Shaders/";
    }
}

