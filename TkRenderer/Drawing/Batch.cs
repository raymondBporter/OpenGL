using System.Diagnostics;
using System.Linq;
using OpenTK.Graphics.OpenGL4;
using TkRenderer.Math.LinearAlgebra;
using TkRenderer.Shapes;
using static TkRenderer.Math.FloatMath;

namespace TkRenderer.Drawing
{
    public class Batch
    {
        public void Draw(Affine worldToDevice)
        {
            if (_chunkBuffer.VertexCount == 0)
                return;

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            BindBuffers();

            _material.Begin(new[]
            {
                worldToDevice.A.A00, worldToDevice.A.A01, worldToDevice.D.X,
                worldToDevice.A.A10, worldToDevice.A.A11, worldToDevice.D.Y,
                0.0f,                0.0f,                1.0f
            });


            _vertexBuffer.SetData(_chunkBuffer.VertexData.ToArray(), BufferUsageHint.StreamDraw);
            _indexBuffer.SetData(_chunkBuffer.IndexData.ToArray(), BufferUsageHint.StreamDraw);
            GL.DrawElements(BeginMode.Triangles, _chunkBuffer.IndexCount, DrawElementsType.UnsignedInt, 0);
        
            _chunkBuffer.Clear();
            _material.End();
            UnBindBuffers();
        }

        private void BindBuffers()
        {
            _vertexArray.Bind();
            _vertexBuffer.Bind();
            _indexBuffer.Bind();
        }

        private static void UnBindBuffers()
        {
            IndexBuffer.UnBind();
            VertexBuffer.UnBind();
            VertexArray.UnBind();
        }



        public void AddGeom(Vector2[] positions, int[] indices, float z, Color4 color, Affine transform, bool useTransform)
        {
            if (useTransform)
            {
                _chunkBuffer.AddChunk(indices, positions, z, color, transform);
            }
            else
            {
                _chunkBuffer.AddChunk(indices, positions, z, color);
            }
        }


        private readonly ChunkBuffer _chunkBuffer = new ChunkBuffer();


        private readonly Material _material = new Material();

        private readonly VertexArray _vertexArray = new VertexArray();
        private readonly VertexBuffer _vertexBuffer = new VertexBuffer();
        private readonly IndexBuffer _indexBuffer = new IndexBuffer();


        private static Vector2[] RegularPolygon(float radius, int numVertices) =>
            new Vector2[numVertices].Select((_, n) => radius * Vector2.FromAngle(2.0f * Pi * n / numVertices)).ToArray();


        public static bool IntersectRects(Rect a, Rect b)
          => !(a.Right < b.Left || a.Left > b.Right || a.Bottom > b.Top || a.Top < b.Bottom);


        private const int NumCircleVerts = 44;
        private readonly Vector2[] _unitCircleVerts = RegularPolygon(1.0f, NumCircleVerts);

        public void DrawCircle(Vector2 pos, float radius, float z, Color4 color, float borderThickness, Color4 borderColor)
        {
            var innerRadius = radius - borderThickness;
            var outer = Affine.Translation(pos) * Affine.Scale(radius);
            var inner = Affine.Translation(pos) * Affine.Scale(innerRadius);

            _chunkBuffer.AddConvexChunk(_unitCircleVerts, z, color, inner);
            _chunkBuffer.AddBorderChunk(_unitCircleVerts.Select(x => inner * x).ToArray(), 
                                        _unitCircleVerts.Select(x => outer * x).ToArray(),
                                        z, borderColor);
        }


        public void DrawCircle(Vector2 pos, float radius, float z, Color4 color)
        {
            _chunkBuffer.AddConvexChunk(_unitCircleVerts, z, color, Affine.Translation(pos) * Affine.Scale(radius));
        }


        public void DrawLine(Vector2 a, Vector2 b, float z, Color4 color, float thickness)
        {
            var normal = (b - a).Normalized.PerpLeft;

            _chunkBuffer.AddConvexChunk(new[] 
            {
                b + thickness * normal / 2.0f,
                b - thickness * normal / 2.0f,
                a - thickness * normal / 2.0f,
                a + thickness * normal / 2.0f
            }, z, color);
        }

        public void DrawRect(Vector2 center, float width, float height, float z, Color4 color)
        {

            _chunkBuffer.AddConvexChunk(new[]
            {
                center + new Vector2( width / 2.0f,  height / 2.0f),
                center + new Vector2(-width / 2.0f,  height / 2.0f),
                center + new Vector2(-width / 2.0f, -height / 2.0f),
                center + new Vector2( width / 2.0f, -height / 2.0f)
            }, z, color);
        }

        public void DrawRect(Rect rect, float z, Color4 color)
        {
            DrawRect(rect.Center, rect.Width, rect.Height, z, color);
        }


        public void DrawRect(Vector2 center, float width, float height, float z, Color4 color, Affine transform)
        {
            _chunkBuffer.AddConvexChunk(new[]
            {
                transform * (center + new Vector2( width / 2.0f,  height / 2.0f)),
                transform * (center + new Vector2(-width / 2.0f,  height / 2.0f)),
                transform * (center + new Vector2(-width / 2.0f, -height / 2.0f)),
                transform * (center + new Vector2( width / 2.0f, -height / 2.0f))
            }, z, color);
        }

        public void DrawTriangle(Triangle triangle, float z, Color4 color)
        {
            DrawTriangle(z, color, triangle[0], triangle[1], triangle[2]);
        }

        public void DrawTriangle(Triangle triangle, float z, Color4 color, Affine transform)
        {
            DrawTriangle(z, color, transform, triangle[0], triangle[1], triangle[2]);
        }

        public void DrawTriangle(float z, Color4 color, params Vector2[] v)
        {
            _chunkBuffer.AddConvexChunk(v, z, color);
        }
        public void DrawTriangle(float z, Color4 color, Affine transform, params Vector2[] v)
        {
            _chunkBuffer.AddConvexChunk(v.Select(x=>transform*x).ToArray(), z, color);
        }

        public void DrawConvexPolygon(ConvexPolygon polygon, float z, Affine transform, Color4 color, Color4 borderColor)
        {
            _chunkBuffer.AddConvexChunk(polygon.InnerPoints, z, color, transform);
            _chunkBuffer.AddBorderChunk(polygon.InnerPoints, polygon.OuterPoints, z, borderColor, transform);
        }




        /*
                private enum Winding { Cw, Ccw }

        public void DrawCone(Vector2 a, Vector2 b, Vector2 c, Vector2 center, float r, float z, Color4 color, int numConeInterpolationVertices)
        {
            AddConvexPolygonIndexData(numConeInterpolationVertices + 1);

            for (int i = 0; i < numConeInterpolationVertices; i++)
            {
                float t = (float) i / (numConeInterpolationVertices - 1);
                AddVertex(center + r * ((1.0f-t) * a + t * c - center).Normalized, z, color);
            }
            AddVertex(b, z, color);
        }

        public void DrawPolyLine(PolyLine polyLine, float z, Color4 color, int numConeInterpolationVertices = 3)
        {
            PreAdd((polyLine.NumPoints-1) * numConeInterpolationVertices - 2);

            float h = polyLine.Thickness / 2;
            Winding winding = Winding.CCW;
            AddVertex(polyLine.Points[0] + h * polyLine.NormalsRh[0], z, color);
            AddVertex(polyLine.Points[0] - h * polyLine.NormalsRh[0], z, color);


            for (int i = 0; i < polyLine.NumEdges - 1; i++)
            {
                Vector2 a = polyLine.Points[i + 0];
                Vector2 b = polyLine.Points[i + 1];
                Vector2 e0 = polyLine.Edge(i + 0);
                Vector2 n0 = polyLine.NormalsRh[i + 0];
                Vector2 n1 = polyLine.NormalsRh[i + 1];
                Winding prevWinding = winding;
                winding = Dot(e0, n1) > 0 ? Winding.CCW : Winding.CW;

                if (winding == Winding.CCW)
                {
                    n0 = -n0;
                    n1 = -n1;
                }

                IndexData[NumIndices + 0] = NumVertices - 1;
                IndexData[NumIndices + 1] = NumVertices - 2;
                IndexData[NumIndices + 2] = NumVertices + 0;
                IndexData[NumIndices + 3] = NumVertices - (winding == prevWinding ? 1 : 2);
                IndexData[NumIndices + 4] = NumVertices + 0;
                IndexData[NumIndices + 5] = NumVertices + numConeInterpolationVertices;
                NumIndices += 6;
                float s = 1.0f + h * (1.0f - Dot(n0, n1)) / Dot(e0, n1);
                DrawCone(
                    b - h * n0, 
                    a + s * e0 + h * n0, 
                    b - h * n1, 
                    b, 
                    h, 
                    z, 
                    color, 
                    numConeInterpolationVertices);
            }

            IndexData[NumIndices + 0] = NumVertices - 2;
            IndexData[NumIndices + 1] = NumVertices - 1;
            IndexData[NumIndices + 2] = NumVertices + 1;
            IndexData[NumIndices + 3] = NumVertices - (winding == Winding.CCW ? 1 : 2);
            IndexData[NumIndices + 4] = NumVertices + 0;
            IndexData[NumIndices + 5] = NumVertices + 1;
            NumIndices += 6;
            AddVertex(polyLine.Points[polyLine.NumPoints - 1] - h * polyLine.NormalsRh[polyLine.NumEdges - 1], z, color);
            AddVertex(polyLine.Points[polyLine.NumPoints - 1] + h * polyLine.NormalsRh[polyLine.NumEdges - 1], z, color);
        }

    */
    }
}