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

            //var hsv = color.AsHSV();
            //hsv.hue += offset;
            //color = hsv.AsColor();

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

            var max = (float)Math.Max(color.R, Math.Max(color.G, color.B));
            var min = (float)Math.Min(color.R, Math.Min(color.G, color.B));

            var hue = color.Hue();
            var saturation = (max == 0) ? 0 : 1f - (1f * min / max);
            var value = max / 255f;

            return (hue, saturation, value);

        }

        public static Color AsColor(this (float hue, float saturation, float value) hsv) =>
            AsColor(hsv.hue, hsv.saturation, hsv.value);

        public static Color AsColor(float hue, float saturation, float value)
        {

            var hi = Convert.ToByte(Math.Floor(hue / 60)) % 6;
            var f = hue / 60 - Math.Floor(hue / 60);

            value *= 255;
            var v = Convert.ToByte(value);
            var p = Convert.ToByte(value * (1 - saturation));
            var q = Convert.ToByte(value * (1 - f * saturation));
            var t = Convert.ToByte(value * (1 - (1 - f) * saturation));

            return hi switch
            {
                1 => Color.FromArgb(255, v, t, p),
                2 => Color.FromArgb(255, q, v, p),
                3 => Color.FromArgb(255, p, v, t),
                4 => Color.FromArgb(255, p, q, v),
                5 => Color.FromArgb(255, t, p, v),
                _ => Color.FromArgb(255, v, p, q),
            };

        }

        public static float Hue(this Color color)
        {

            float min = Math.Min(Math.Min(color.R, color.G), color.B);
            float max = Math.Max(Math.Max(color.R, color.G), color.B);

            if (min == max)
                return 0;

            float hue;
            if (max == color.R)
                hue = (color.G - color.B) / (max - min);
            else if (max == color.G)
                hue = 2f + (color.B - color.R) / (max - min);
            else
                hue = 4f + (color.R - color.G) / (max - min);

            hue *= 60;
            if (hue < 0) 
                hue += 360;

            return (float)Math.Round(hue);

        }

    }

}
