using System.Linq;
using TkRenderer.Drawing;
using TkRenderer.Math.LinearAlgebra;
using TkRenderer.Shapes;
using static TkRenderer.Math.FloatMath;

namespace TkRenderer
{
    public class VisualFactory
    {
        public static Visual RectVisual(float width, float height, Color4 color, float z, bool useTransform = true) =>
            new Visual(
                RectVertices(width, height), 
                RectFillIndices, 
                color, 
                z, 
                useTransform, 
                RectBoundingRect(width, height));

        public static Visual CircleVisual(float radius, Color4 color, float z, bool useTransform = true) =>
            new Visual(
                CircleVertices(radius, NumCircleVerts),
                CircleFillIndices,
                color,
                z,
                useTransform,
                CircleBoundingRect(radius));

        public static Visual CapsuleVisual(float length, float radius, Color4 color, float z) =>
            new Visual(
                CapsuleVertices(length, radius),
                CapsuleFillIndices,
                color,
                z,
                true,
                CapsuleBoundingRect(length, radius));


        public static Visual RandomVisual()
        {
           // float randomZ = Rand(1.0f);
            //float rand = Rand(4);
           // if (rand > 3.0f)
          //      return RectVisual(4.0f, 0.4f, new Color4(0.0f, 1.0f, 1.0f, 1.0f), LineMaterial, 0.1f);
         //   if (rand > 2.4f)
                 return RectVisual(4f, .4f, new Color4(1.0f, 0.0f, 0.0f, 1.0f), 0.2f);
           // if (rand > 2.0f)
          //      return RectVisual(4f, 0.4f, new Color4(0.0f, 1.0f, 0.0f, 1.0f), LineMaterial, 0.3f);
            //if (rand > 1.6f)
             //   return CircleVisual(2f + Rand(1.1f), new Color4(0.3f, 1.0f, 0.4f, 0.3f), FillMaterial, 0.4f);
          //  if (rand > 0.1f)
             //   return CircleVisual(2.1f, new Color4(1.0f, 0.0f, 0.0f, 1.0f), FillMaterial, 0.5f);
            //else//if (rand > .05f)
              //  return CircleVisual(3.1f + Rand(0.1f), new Color4(1.0f, 1.0f, 0.0f, 1.0f), FillMaterial, 0.6f);
          //  else
          //      return RectVisual(2.2f, 1.4f, new Color4(0.0f, 0.0f, 1.0f, 1.0f), TextureMaterial("table.png"), 0.7f);
        }

        private const int NumCircleVerts = 64;




        public static Vector2[] CircleVertices(float radius, int numVertices) =>
            new Vector2[numVertices].Select((_, n) => radius * Vector2.FromAngle(2.0f * Pi * n / numVertices)).ToArray();

        public static Vector2[] CapsuleVertices(float length, float radius)
        {
            var verts = new Vector2[NumCircleVerts + 2];
            for (var i = 0; i < NumCircleVerts / 2 + 1; i++)
            {
                verts[i] = radius * Vector2.FromAngle(-Pi / 2.0f + Pi * i / (NumCircleVerts / 2.0f)) + new Vector2(length / 2.0f, 0);
            }
            for (var i = 0; i < NumCircleVerts / 2 + 1; i++)
            {
                verts[NumCircleVerts / 2 + i] = radius * Vector2.FromAngle(Pi / 2.0f + Pi * i / (NumCircleVerts / 2.0f)) + new Vector2(-length / 2.0f, 0);
            }
            return verts;
        }

        private static Vector2[] RectVertices(float w, float h) => new[] 
            {   
                0.5f * new Vector2(-w, -h),  //BL
                0.5f * new Vector2( w, -h),  //BR
                0.5f * new Vector2( w,  h),  //TR
                0.5f * new Vector2(-w,  h)   //TL
            };


        private static readonly int[] RectFillIndices = ConvexFillIndices(4);
        private static readonly int[] CircleFillIndices = ConvexFillIndices(NumCircleVerts);
        private static readonly int[] CapsuleFillIndices = ConvexFillIndices(NumCircleVerts + 2);


        /*
            Triangle fan indices (0, 1, 2), (0, 2, 3), (0, 3, 4),...
        */
        private static int[] ConvexFillIndices(int numVerts)
        {
            var list = new int[(numVerts - 1) * 3];
            for (var i = 1; i < numVerts - 1; i++)
            {
                list[i * 3] = 0;
                list[i * 3 + 1] = i;
                list[i * 3 + 2] = i + 1;
            }
            return list;
        }


        private static Rect RectBoundingRect(float w, float h)
        {
            var r = Sqrt(w * w / 4 + h * h / 4);
            return new Rect(Vector2.Zero, 2 * r, 2 * r);
        }

        private static Rect CircleBoundingRect(float r) => new Rect(Vector2.Zero, 2 * r, 2 * r);

        private static Rect CapsuleBoundingRect(float l, float r) => new Rect(Vector2.Zero, 2 * (l + r), 2 * (l + r));


        private static (float Min, float Max) MinMax(float a, float b) => a < b ? (a, b) : (b, a);


        public static Rect LineBoundingRect(Vector2 a, Vector2 b)
        {
            var (left, right) = MinMax(a.X, b.X);
            var (bottom, top) = MinMax(a.Y, b.Y);

            return new Rect(left, right, bottom, top);
        }       
    }
}

