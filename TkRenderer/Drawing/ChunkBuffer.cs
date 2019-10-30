using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TkRenderer.Math.LinearAlgebra;

namespace TkRenderer.Drawing
{
    public class ChunkBuffer
    {
        public List<float> VertexData = new List<float>();
        
        public List<int> IndexData = new List<int>();

        private const int VertexSize = 7;
        public int VertexCount => VertexData.Count / VertexSize;
        public int IndexCount => IndexData.Count;

        public void AddChunk(int[] indices, Vector2[] positions, float z, Color4 color, Affine transform)
        {
            AddIndices(indices);
            AddVertices(positions, z, color, transform);
        }

        public void AddChunk(int[] indices, Vector2[] positions, float z, Color4 color)
        {
            AddIndices(indices);
            AddVertices(positions, z, color);
        }

        public void AddConvexChunk(Vector2[] positions, float z, Color4 color, Affine transform)
        {
            AddConvexIndices(positions.Length);
            AddVertices(positions, z, color, transform);
        }

        public void AddConvexChunk(Vector2[] positions, float z, Color4 color)
        {
            AddConvexIndices(positions.Length);
            AddVertices(positions, z, color);
        }

        public void AddBorderChunk(Vector2[] innerPositions, Vector2[] outerPositions, float z, Color4 color, Affine transform)
        {
            Debug.Assert(innerPositions.Length == outerPositions.Length);
            AddBorderIndices(innerPositions.Length);
            for (var i = 0; i < innerPositions.Length; i++)
            {
                AddVertex(transform * innerPositions[i], z, color);
                AddVertex(transform * outerPositions[i], z, color);
            }
        }

        public void AddBorderChunk(Vector2[] innerPositions, Vector2[] outerPositions, float z, Color4 color)
        {
            Debug.Assert(innerPositions.Length == outerPositions.Length);
            AddBorderIndices(innerPositions.Length);
            for (var i = 0; i < innerPositions.Length; i++)
            {
                AddVertex(innerPositions[i], z, color);
                AddVertex(outerPositions[i], z, color);
            }
        }


        public void AddVertex(Vector2 position, float z, Color4 color)
        {
            VertexData.Add(position.X);
            VertexData.Add(position.Y);
            VertexData.Add(z);
            VertexData.Add(color.R);
            VertexData.Add(color.G);
            VertexData.Add(color.B);
            VertexData.Add(color.A);
        }

        public void AddVertices(Vector2[] positions, float z, Color4 color)
        {
            for (var i = 0; i < positions.Length; i++)
            {
                VertexData.Add(positions[i].X);
                VertexData.Add(positions[i].Y);
                VertexData.Add(z);
                VertexData.Add(color.R);
                VertexData.Add(color.G);
                VertexData.Add(color.B);
                VertexData.Add(color.A);
            }
        }

        public void AddVertices(Vector2[] positions, float z, Color4 color, Affine transform)
        {
            AddVertices(positions.Select(x => transform * x).ToArray(), z, color);
        }

        public void AddConvexIndices(int polyVertexCount)
        {
            for (var i = 1; i < polyVertexCount - 1; i++)
            {
                IndexData.Add(VertexCount + 0);
                IndexData.Add(VertexCount + i);
                IndexData.Add(VertexCount + i + 1);
            }
        }

        public void AddIndices(int[] indices)
        {
            for (var i = 0; i < indices.Length; i++)
            {
                IndexData.Add(VertexCount + i);
            }
        }

        private void AddBorderIndices(int numPolyVertices)
        {
            for (var i = 0; i < numPolyVertices - 1; i++)
            {
                IndexData.Add(2 * i + 0 + VertexCount);
                IndexData.Add(2 * i + 1 + VertexCount);
                IndexData.Add(2 * i + 2 + VertexCount);
                IndexData.Add(2 * i + 2 + VertexCount);
                IndexData.Add(2 * i + 3 + VertexCount);
                IndexData.Add(2 * i + 1 + VertexCount);
            }
            IndexData.Add(2 * (numPolyVertices - 1) + 0 + VertexCount);
            IndexData.Add(2 * (numPolyVertices - 1) + 1 + VertexCount);
            IndexData.Add(                            0 + VertexCount);
            IndexData.Add(                            0 + VertexCount);
            IndexData.Add(2 * (numPolyVertices - 1) + 1 + VertexCount);
            IndexData.Add(                            1 + VertexCount);
        }

        public void Clear()
        {
            VertexData.Clear();
            IndexData.Clear();
        }
    }
}