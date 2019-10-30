using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TkRenderer.Math;
using TkRenderer.Math.LinearAlgebra;
using static TkRenderer.Math.LinearAlgebra.Vector2;


namespace TkRenderer.Shapes
{
    public class ConvexPolygon : IShape
    {
        public int NumVertices => OuterPoints.Length;

        public Vector2[] OuterPoints { get; }
        public Vector2[] InnerPoints { get; }
        public Vector2[] Normal { get; }

        public Vector2 Centroid => _AreaCentroid().Item2;

        private (float, Vector2) _AreaCentroid( )
        {
            Vector2 centroid = Vector2.Zero;
            float area = 0.0f;
            for (int i = 1; i < OuterPoints.Length - 1; i++)
            {
                int i0 = 0;
                int i1 = 1;
                int i2 = i + 1;

                Vector2 v0 = OuterPoints[i0];
                Vector2 v1 = OuterPoints[i1];
                Vector2 v2 = OuterPoints[i2];

                Vector2 cenTri = (v0 + v1 + v2) / 3.0f;
                float areaTri = 0.5f * FloatMath.Fabs(Vector2.Dot(v1 - v0, (v2 - v0).PerpLeft));

                area += areaTri;
                centroid += cenTri * areaTri;
            }

            centroid /= area;
            return (area, centroid);
        }


        public float Area => _AreaCentroid().Item1;

        public float Perimeter => Edges.Select(e=>e.Length).Sum();

        public Vector2 Edge(int i) =>
            i == NumVertices - 1
            ? OuterPoints[0] - OuterPoints[NumVertices - 1] 
            : //if ( i >= 0 && i < NumVertices - 1)
            OuterPoints[i + 1] - OuterPoints[i];


        public List<Vector2> Edges => OuterPoints.Skip(1).Select((v, i) => (v -OuterPoints[i-1])).ToList();


        public Vector2 EdgeDir(int i) => Normal[i].PerpLeft;


        public float BorderThickness;


        public ConvexPolygon(Vector2[] vertices, float borderThickness)
        {
            BorderThickness = borderThickness;
            Debug.Assert(vertices.Length >= 3);

            OuterPoints = vertices;
            
            Normal = new Vector2[NumVertices];

            for (var i = 0; i < NumVertices; i++)
            {
                Normal[i] = Edge(i).PerpRight.Normalized;
            }

            Debug.Assert(IsConvexCcw());

            InnerPoints = new Vector2[NumVertices];

            CreateInnerPoints();
        }


        //TODO: will fail on self intersecting polygons
        private bool IsConvexCcw()
        {
            var next = OuterPoints[1] - OuterPoints[0];

            for ( var i = 0; i < NumVertices-2; i++ )
            {
                var cur = next;
                next = OuterPoints[i + 2] - OuterPoints[i + 1];

                if (Dot(cur.PerpLeft, next) <= 0)
                {
                    return false;
                }
            }

            return true;
            //return Dot(next, (Vertices[1] - Vertices[0]).PerpLeft) > 0;
        }

        public int NextIndex(int index) => index == NumVertices - 1 ? 0 : index + 1;


        public int PrevIndex(int index) => index == 0 ? NumVertices - 1 : index - 1;


        private void CreateInnerPoints()
        {
            for (var i = 0; i < NumVertices; i++)
            {
                InnerPoints[i] = InnerPoint(PrevIndex(i), i, BorderThickness);
            }
        }


        private Vector2 InnerPoint(int i, int j, float d)
        {
            // Let E0, E1 be segments parameterized by
            // E0(t) = p0 + t e0 and E1(s) = p1 + s e1 for s, t in [0, 1]
            // with E0(1) = p1

            // p0 + t e0 + d n0 = p1 + s e1 + d n1
            // dot(e0, n0) = 0
            // dot(p0, n0) + d = dot(p1, n0) + s dot(e1, n0) + d dot(n1, n0)
            // s = (p0 - p1)n0 + d( 1 - n0n1) / e1n0


            var ni = Normal[i];
            var nj = Normal[j];
            var ei = Edge(i);
            var ej = Edge(j);
            var pj = OuterPoints[j];


            var s = -(Dot(ei, ni) + d * (1.0f - Dot(ni, nj))) / Dot(ej, ni);
            return pj + s * ej - d * nj;
        }



    }


    public class PolyLine
    {
        public float Thickness = 4.0f;

        public PolyLine(IReadOnlyList<Vector2> points)
        {
            Points = new Vector2[points.Count];

            for ( var i = 0; i < points.Count; i++ )
            {
                Points[i] = points[i];
            }
            NormalsRh = new Vector2[points.Count-1];

            for ( var i = 0; i < NumEdges; i++ )
            {
                NormalsRh[i] = Edge(i).Normalized.PerpRight;
            }
        }


        // n - 1 edges
        public Vector2 Edge(int i) => Points[i + 1] - Points[i];
        public Vector2[] NormalsRh;
        public Vector2[] Points;

        public int NumPoints => Points.Length;
        public int NumEdges => Points.Length - 1;
    }
}
