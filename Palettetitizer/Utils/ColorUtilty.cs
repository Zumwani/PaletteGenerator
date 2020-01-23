using System;
using System.Windows.Media;

namespace PaletteGenerator
{

    public static class ColorUtilty
    {

        public static Color Blend(this Color color, Color backColor, float amount)
        {
            byte a = (byte)((color.A * amount) + backColor.A * (1 - amount));
            byte r = (byte)((color.R * amount) + backColor.R * (1 - amount));
            byte g = (byte)((color.G * amount) + backColor.G * (1 - amount));
            byte b = (byte)((color.B * amount) + backColor.B * (1 - amount));
            return Color.FromArgb(a, r, g, b);
        }

        public static Color ComplementaryColor(this Color color)
        {
            var hsv = color.AsHSV();
            hsv.hue = (hsv.hue + 0.5f) % 1.0f;
            return hsv.AsColor();
        }

        public static (double hue, double saturation, double value) AsHSV(this Color color)
        {

            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            var hue = color.Hue();
            var saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            var value = max / 255d;

            return (hue, saturation, value);

        }

        public static Color AsColor(this (double hue, double saturation, double value) hsv)
        {

            int hi = Convert.ToInt32(Math.Floor(hsv.hue / 60)) % 6;
            double f = hsv.hue / 60 - Math.Floor(hsv.hue / 60);

            hsv.value *= 255;
            byte v = Convert.ToByte(hsv.value);
            byte p = Convert.ToByte(hsv.value * (1 - hsv.saturation));
            byte q = Convert.ToByte(hsv.value * (1 - f * hsv.saturation));
            byte t = Convert.ToByte(hsv.value * (1 - (1 - f) * hsv.saturation));

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

        public static float Hue(this Color c) =>
            System.Drawing.Color.FromArgb(c.A, c.R, c.G, c.B).GetHue();

    }

}
