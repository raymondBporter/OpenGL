namespace TkRenderer
{
    internal static class HashCodeHelper
    {
        internal static int CombineHashCodes(int h1, int h2) => ((h1 << 5) + h1) ^ h2;
        internal static int CombineHashCodes(int h1, int h2, int h3) => CombineHashCodes(CombineHashCodes(h1, h2), h3);
        internal static int CombineHashCodes(int h1, int h2, int h3, int h4) => CombineHashCodes(CombineHashCodes(CombineHashCodes(h1, h2), h3), h4);
        internal static int GetHashCode<T>(ref T a, ref T b) => CombineHashCodes(a.GetHashCode(), b.GetHashCode());
        internal static int GetHashCode<T>(ref T a, ref T b, ref T c) => CombineHashCodes(a.GetHashCode(), b.GetHashCode(), c.GetHashCode());
        internal static int GetHashCode<T>(ref T a, ref T b, ref T c, ref T d) => CombineHashCodes(a.GetHashCode(), b.GetHashCode(), c.GetHashCode(), d.GetHashCode());
        internal static int GetHashCode<T>(T a, T b) => CombineHashCodes(a.GetHashCode(), b.GetHashCode());
        internal static int GetHashCode<T>(T a, T b, T c) => CombineHashCodes(a.GetHashCode(), b.GetHashCode(), c.GetHashCode());
        internal static int GetHashCode<T>(T a, T b, T c, T d) => CombineHashCodes(a.GetHashCode(), b.GetHashCode(), c.GetHashCode(), d.GetHashCode());
    }
}