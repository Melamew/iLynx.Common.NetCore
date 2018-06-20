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

        public static double CubicIn(double t)
        {
            return t * t * t;
        }

        public static double CubicOut(double t)
        {
            return (t - 1d) * t * t + 1d;
        }

        public static double CubicInOut(double t)
        {
            return t < .5d ? 4d * t * t * t : (t - 1d) * (2d * t - 2d) * (2d * t - 2d) + 1d;
        }
    }
}