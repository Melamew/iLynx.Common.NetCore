using System.Runtime.InteropServices;
using iLynx.Common.Maths;
using SFML.System;

namespace iLynx.UI.Sfml
{
    [StructLayout(LayoutKind.Sequential)]
    public struct LineSegment
    {
        public Vector2f P1;
        public Vector2f P2;

        public LineSegment(Vector2f p1, Vector2f p2)
        {
            P1 = p1;
            P2 = p2;
        }

        public LineSegment(float x1, float y1, float x2, float y2)
        {
            P1 = new Vector2f(x1, y1);
            P2 = new Vector2f(x2, y2);
        }

        public LineSegment(float x1, float y1, Vector2f p2)
        {
            P1 = new Vector2f(x1, y1);
            P2 = p2;
        }

        public LineSegment(Vector2f p1, float x2, float y2)
        {
            P1 = p1;
            P2 = new Vector2f(x2, y2);
        }

        public Intersect Intersects(LineSegment other)
        {
            return Math2D.AreIntersecting(P1.X, P1.Y, P2.X, P2.Y, other.P1.X, other.P1.Y, other.P2.X, other.P2.Y);
        }
    }
}