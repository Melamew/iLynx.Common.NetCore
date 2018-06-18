namespace iLynx.UI.Sfml.Animation
{
    public static class EasingFunctions
    {
        public static double Linear(double t)
        {
            return t;
        }

        public static double QuadraticIn(double t)
        {
            return t * t;
        }

        public static double QuadraticOut(double t)
        {
            return t * (2 - t);
        }

        public static double QuadraticInOut(double t)
        {
            return t > .5d ? -1d + (4d - 2d * t) * t : 2d * t * t;
        }

        public static double CubicEaseIn(double t)
        {
            return t * t * t;
        }
    }
}