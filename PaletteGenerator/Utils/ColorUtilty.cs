using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace PaletteGenerator
{

    public static class ColorUtilty
    {

        public static IEnumerable<Color> Blend(this Color from, Color to, int steps)
        {

            var stepA = (byte)((to.A - from.A) / (steps - 1));
            var stepR = (byte)((to.R - from.R) / (steps - 1));
            var stepG = (byte)((to.G - from.G) / (steps - 1));
            var stepB = (byte)((to.B - from.B) / (steps - 1));

            return steps.Select(i =>
            Color.FromArgb(
                (byte)(from.A + (stepA * i)),
                (byte)(from.R + (stepR * i)),
                (byte)(from.G + (stepG * i)),
                (byte)(from.B + (stepB * i))));

        }

        public static Color OffsetHue(this Color color, float offset)
        {

            //TODO: Implement hue

            var hsv = color.AsHSV();
            hsv.hue += offset;
            color = hsv.AsColor();

            return color;

        }

        public static Color Complementary(this Color color)
        {
            var hsv = color.AsHSV();
            hsv.hue = (hsv.hue + 0.5f) % 1f;
            return hsv.AsColor();
        }

        public static (float hue, float saturation, float value) AsHSV(this Color color)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            var hue = color.Hue();
            var saturation = (max == 0) ? 0 : 1f - (1f * min / max);
            var value = max / 255f;

            return (hue, saturation, value);

        }

        public static float Hue(this Color color) =>
            System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B).GetHue();

        public static Color AsColor(this (float hue, float saturation, float value) hsv) =>
            AsColor(hsv.hue, hsv.saturation, hsv.value);

        public static Color AsColor(float hue, float saturation, float value)
        {

            var hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value *= 255;
            var v = (byte)Convert.ToInt32(value);
            var p = (byte)Convert.ToInt32(value * (1 - saturation));
            var q = (byte)Convert.ToInt32(value * (1 - f * saturation));
            var t = (byte)Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }

    }

}
