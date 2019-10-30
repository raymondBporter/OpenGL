using System.Runtime.InteropServices;
using static TkRenderer.Math.FloatMath;

namespace TkRenderer.Drawing
{
	[StructLayout(LayoutKind.Sequential)]
	public struct Color4 
	{
        public float R;
        public float G;
        public float B;
        public float A;

        public static Color4 White => new Color4(1.0f, 1.0f, 1.0f);
        public static Color4 Black => new Color4(0.0f, 0.0f, 0.0f);
        public static Color4 Red =>   new Color4(1.0f, 0.0f, 0.0f);
        public static Color4 Green => new Color4(0.0f, 1.0f, 0.0f);
        public static Color4 Blue =>  new Color4(0.0f, 0.0f, 1.0f);
        public static Color4 Yellow => new Color4(1.0f, 1.0f, 0.0f);
        public static Color4 Cyan => new Color4(0.0f, 1.0f, 1.0f);



        public Color4(float red, float green, float blue, float alpha)
		{
			R	= red;
			G	= green;
			B	= blue;
			A	= alpha;
		}

        public Color4(float red, float green, float blue) : this(red, green, blue, 1.0f) { }

        public void Clamp()
		{
            R = Clamp01(R);
            G = Clamp01(G);
            B = Clamp01(B);
            A = Clamp01(A);
		}

        public override bool Equals(object obj) => obj is Color4 c && this == c;
        public override int GetHashCode() => HashCodeHelper.GetHashCode(R, G, B, A);
        public float Intensity => (R + G + B) / 3.0f;
        public override string ToString() => $"Color4({R}, {G}, {B}, {A})";
        public static bool operator==(Color4 u, Color4 v) => u.R == v.R && u.G == v.G && u.B == v.B && u.A == v.A;
        public static bool operator!=(Color4 u, Color4 v) => !(u==v);
        public static explicit operator float[] (Color4 color) => new[] { color.R, color.G, color.B, color.A };
	}
}
