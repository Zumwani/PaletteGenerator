using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using color = System.Drawing.Color;
using Color = System.Windows.Media.Color;

namespace PaletteGenerator
{

    public static class ColorUtilty
    {

        public static Color ApplyOffsets(this Color color)     => color.ApplyOffsets(App.Window?.Hue ?? 0, App.Window?.Saturation ?? 0);
        public static Color[] ApplyOffsets(this Color[] colors) => colors.ApplyOffsets(App.Window?.Hue ?? 0, App.Window?.Saturation ?? 0);

        public static Color ApplyOffsets(this Color color, float hue, float saturation)      => color.OffsetHue(hue).OffsetSaturation(saturation);
        public static Color[] ApplyOffsets(this Color[] colors, float hue, float saturation) => colors.Select(c => c.ApplyOffsets(hue, saturation)).ToArray();

        public static BitmapSource AsPNGPalette(this Color[][] colors, int cellSize)
        {

            if (colors.Length == 0)
                return null;

            for (int i = 0; i < colors.Length; i++)
                colors[i] = colors[i].SelectMany(c => Enumerable.Repeat(c, cellSize)).ToArray();

            colors = colors.SelectMany(c => Enumerable.Repeat(c, cellSize)).ToArray();

            var width = colors[0].Length;
            var height = colors.Length;

            var pixelFormat = PixelFormats.Bgra32;
            var stride = width * 4; // bytes per row

            byte[] pixelData = new byte[height * stride];

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {

                    var color = colors[y][x];
                    var index = (y * stride) + (x * 4);

                    pixelData[index] = color.B;
                    pixelData[index + 1] = color.G;
                    pixelData[index + 2] = color.R;
                    pixelData[index + 3] = color.A;

                }

            var bitmap = BitmapSource.Create(width, height, 96, 96, pixelFormat, null, pixelData, stride);
            return bitmap;

        }

        public static color AsDrawing(this Color color) =>
            System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);

        public static MemoryStream AsBytes(this BitmapSource bitmap)
        {

            var ms = new MemoryStream();
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            encoder.Save(ms);
            return ms;

        }

        public static MemoryStream AsBytes(this Bitmap bitmap)
        {
            var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            return ms;
        }

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

            var hsv = color.AsHSV();
            hsv.hue = MathUtility.Wrap(hsv.hue + (offset * 360), 0, 360);
            color = hsv.AsColor();

            return color;

        }

        public static Color OffsetSaturation(this Color color, float offset)
        {

            var hsv = color.AsHSV();
            hsv.saturation = (hsv.saturation * offset).Clamp01();
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
