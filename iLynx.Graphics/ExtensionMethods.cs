using OpenTK;

namespace iLynx.Graphics
{
    public static class ExtensionMethods
    {
        public static Vector2 AsVector(this PointF point)
        {
            return new Vector2(point.X, point.Y);
        }

        public static PointF AsPoint(this Vector2 vector)
        {
            return new PointF(vector.X, vector.Y);
        }
    }
}
