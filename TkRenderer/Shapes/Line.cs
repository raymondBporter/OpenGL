using System;
using TkRenderer.Math;
using TkRenderer.Math.LinearAlgebra;

namespace TkRenderer.Shapes
{

    // L(t) = P + t D
    // |D| = 1
    // t in (-inf, inf)
    public class Line
    {
        public Vector2 Origin, Direction;

        public Line()
        {
            Origin = Vector2.Zero;
            Direction = Vector2.Zero;
        }

        public Line(Vector2 origin, Vector2 direction)
        {
            Origin = origin;
            Direction = direction;

            //           Debug.Assert(FloatMath.Fabs(Direction.LengthSq-1.0f) < 0.0001f); 

        }
    }

    // L(t) = P + t D
    // |D| = 1
    // t in [0, inf)
    public class Ray
    {
        public Vector2 Origin, Direction;

        public Ray()
        {
            Origin = Vector2.Zero;
            Direction = Vector2.Zero;
        }

        public Ray(Vector2 origin, Vector2 direction)
        {
            Origin = origin;
            Direction = direction;

            //           Debug.Assert(FloatMath.Fabs(Direction.LengthSq-1.0f) < 0.0001f); 

        }
    }




    public class TestLineLine
    {
        public struct TestLineLineResult
        {
            public bool Intersect;

            public int NumIntersections;
        };

        public TestLineLineResult Intersect(Line line0, Line line1)
        {
            TestLineLineResult lineLineResult;

            // The intersection of two lines is a solution to P0 + s0*D0 = P1 + s1*D1.
            // Rewrite this as s0*D0 - s1*D1 = P1 - P0 = Q.  If DotPerp(D0, D1)) = 0,
            // the lines are parallel.  Additionally, if DotPerp(Q, D1)) = 0, the
            // lines are the same.  If Dotperp(D0, D1)) is not zero, then
            //   s0 = DotPerp(Q, D1))/DotPerp(D0, D1))
            // produces the point of intersection.  Also,
            //   s1 = DotPerp(Q, D0))/DotPerp(D0, D1))

            Vector2 diff = line1.Origin - line0.Origin;
            float D0DotPerpD1 = Vector2.Dot(line0.Direction.PerpLeft, line1.Direction);
            if (D0DotPerpD1 != 0.0f)
            {
                // The lines are not parallel.
                lineLineResult.Intersect = true;
                lineLineResult.NumIntersections = 1;
            }
            else
            {
                // The lines are parallel.
                diff.Normalize();
                float diffNDotPerpD1 = Vector2.Dot(diff.PerpLeft, line1.Direction);
                if (diffNDotPerpD1 != 0.0f)
                {
                    // The lines are parallel but distinct.
                    lineLineResult.Intersect = false;
                    lineLineResult.NumIntersections = 0;
                }
                else
                {
                    // The lines are the same.
                    lineLineResult.Intersect = true;
                    lineLineResult.NumIntersections = int.MaxValue;
                }
            }

            return lineLineResult;
        }
    };


    public static class FindLineLine
    {
        public struct FindLineLineResult
        {

            public bool Intersect;
            public int NumIntersections;

            public float Line0Parameter;
            public float Line1Parameter;
            public Vector2 Point;
        };

        public static FindLineLineResult Intersect(Line line0, Line line1)
        {
            FindLineLineResult lineLineResult = new FindLineLineResult();


            Vector2 diff = line1.Origin - line0.Origin;
            float D0DotPerpD1 = Vector2.Dot(line0.Direction.PerpLeft, line1.Direction);
            if (D0DotPerpD1 != 0.0f)
            {
                // The lines are not parallel.
                lineLineResult.Intersect = true;
                lineLineResult.NumIntersections = 1;
                float invD0DotPerpD1 = 1.0f / D0DotPerpD1;
                float diffDotPerpD0 = Vector2.Dot(diff.PerpLeft, line0.Direction);
                float diffDotPerpD1 = Vector2.Dot(diff.PerpLeft, line1.Direction);
                float s0 = diffDotPerpD1 * invD0DotPerpD1;
                float s1 = diffDotPerpD0 * invD0DotPerpD1;
                lineLineResult.Line0Parameter = s0;
                lineLineResult.Line1Parameter = s1;
                lineLineResult.Point = line0.Origin + s0 * line0.Direction;
            }
            else
            {
                // The lines are parallel.
                diff.Normalize();
                float diffNDotPerpD1 = Vector2.Dot(diff.PerpLeft, line1.Direction);
                if (FloatMath.Fabs(diffNDotPerpD1) != 0.0f)
                {
                    // The lines are parallel but distinct.
                    lineLineResult.Intersect = false;
                    lineLineResult.NumIntersections = 0;
                }
                else
                {
                    // The lines are the same.
                    lineLineResult.Intersect = true;
                    lineLineResult.NumIntersections = Int32.MaxValue;
                    float maxfloat = float.MaxValue;
                    lineLineResult.Line0Parameter = maxfloat;
                    lineLineResult.Line1Parameter = maxfloat;
                }
            }

            return lineLineResult;
        }


    };

class TestLineRay
{
    public struct TestLineRayResult
    {
        public TestLineRayResult(bool intersects, int numIntersections)
        {
            Intersect = intersects;
            NumIntersections = numIntersections;
        }

        public bool Intersect;

            // The number is 0 (no intersection), 1 (line and ray intersect in a
            // single point) or std::numeric_limits<int>::max() (line and ray
            // are collinear).
        public int NumIntersections;
    };


    TestLineRayResult Intersect(Line line, Ray ray)
    {
        FindLineLine.FindLineLineResult llResult = FindLineLine.Intersect(line, new Line(ray.Origin, ray.Direction));

        if (llResult.NumIntersections == 1)
        {
            // Test whether the line-line intersection is on the ray.
            if (llResult.Line1Parameter >= 0)
            {
                return new TestLineRayResult(true, 1);
            }
            else
            {
                return new TestLineRayResult(false, 0);
            }
        }
        else
        {
            return new TestLineRayResult(llResult.Intersect, llResult.NumIntersections);
        }
    }

};

public static class FindLineRay 
{
    public struct FindLineRayResult 
    {
        public bool Intersect;

        public int NumIntersections;
        public float LineParameter;
        public float RayParameter;
        public Vector2 Point;
    };



    public static FindLineRayResult Intersect(Line line, Ray ray)
    {
        FindLineRayResult result = new FindLineRayResult();
        var llResult = FindLineLine.Intersect(line, new Line(ray.Origin, ray.Direction));
        
        if (llResult.NumIntersections == 1)
        {
            // Test whether the line-line intersection is on the ray.
            if (llResult.Line1Parameter >= 0)
            {
                result.Intersect = true;
                result.NumIntersections = 1;
                result.LineParameter = llResult.Line0Parameter;
                result.RayParameter = llResult.Line1Parameter;
                result.Point = llResult.Point;
            }
            else
            {
                result.Intersect = false;
                result.NumIntersections = 0;
                result.LineParameter = float.NaN;
                result.RayParameter = float.NaN;
            }
        }
        else if (llResult.NumIntersections == int.MaxValue)
        {
            result.Intersect = true;
            result.NumIntersections = int.MaxValue;
            result.LineParameter = float.PositiveInfinity;
            result.RayParameter = float.PositiveInfinity;
        }
        else
        {
            result.Intersect = false;
            result.NumIntersections = 0;
            result.LineParameter = float.NaN;
            result.RayParameter = float.NaN;
        }
        return result;
    }


};




}
