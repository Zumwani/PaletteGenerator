using System;

namespace PaletteGenerator.Utilities
{

    /// <summary>Contains useful math functions.</summary>
    static class MathUtility
    {

        public static int Clamp(this int value, int min, int max)
        {
            if (value > max) value = max;
            if (value < min) value = min;
            return value;
        }

        public static float Clamp(this float value, float min, float max)
        {
            if (value > max) value = max;
            if (value < min) value = min;
            return value;
        }

        public static double Clamp(this double value, double min, double max)
        {
            if (value > max) value = max;
            if (value < min) value = min;
            return value;
        }

        public static float Clamp01(this float value) =>
            Clamp(value, 0, 1);

        public static void Clamp01(ref float value) =>
            Clamp01(value);

        public static float Wrap(this float value, float min, float max) =>
            value - (max - min) * MathF.Floor(value / (max - min));

    }

}
