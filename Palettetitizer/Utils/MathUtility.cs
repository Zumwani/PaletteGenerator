namespace Palettetitizer
{
    public static class MathUtility
    {

        public static int Clamp(int value, int min, int max)
        {
            if (value > max) value = max;
            if (value < min) value = min;
            return value;
        }

        public static float Clamp(float value, float min, float max)
        {
            if (value > max) value = max;
            if (value < min) value = min;
            return value;
        }

        public static double Clamp(double value, double min, double max)
        {
            if (value > max) value = max;
            if (value < min) value = min;
            return value;
        }

    }

}
