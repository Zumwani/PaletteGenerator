using PaletteGenerator.Models;
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

namespace PaletteGenerator.Utilities
{

    /// <summary>Contains utility functions for working with <see cref="Color"/>.</summary>
    static class ColorUtilty
    {

        /// <summary>Returns this <see cref="Color"/> as <see cref="System.Drawing.Color"/>.</summary>
        public static color AsDrawing(this Color color) =>
            System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);

        /// <summary>Returns an array of <see cref="Color"/> that is a blend between start and end colors.</summary>
        public static IEnumerable<Color> Blend(this Color from, Color to, int steps)
        {

            steps = steps.Clamp(2, int.MaxValue);

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

        /// <summary>Returns a complementary color. i.e. if <see cref="Colors.White"/> is specified then <see cref="Colors.Black"/> would be returned.</summary>
        public static Color Complementary(this Color color)
        {
            var hsv = color.AsHSV();
            hsv.hue = (hsv.hue + 0.5f) % 1f;
            return hsv.AsColor();
        }

        /// <summary>Gets the hue of this <see cref="Color"/>.</summary>
        public static float Hue(this Color color) =>
            System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B).GetHue();

        #region Export

        /// <summary>Returns this multidimensional <see cref="Color"/> array as a color palette png file.</summary>
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

        /// <summary>Converts this <see cref="BitmapSource"/> to an <see cref="Byte"/> array, wrapped in an <see cref="MemoryStream"/>.</summary>
        public static MemoryStream AsBytes(this BitmapSource bitmap)
        {

            var ms = new MemoryStream();
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            encoder.Save(ms);
            return ms;

        }

        /// <summary>Converts this <see cref="Bitmap"/> to an <see cref="Byte"/> array, wrapped in an <see cref="MemoryStream"/>.</summary>
        public static MemoryStream AsBytes(this Bitmap bitmap)
        {
            var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            return ms;
        }

        #endregion
        #region Apply Hue shift / Hue offset / Saturation

        /// <summary>Applies <see cref="Global"/> hue shift, hue offset and saturation level to this <see cref="Color"/>.</summary>
        public static Color ApplyOffsets(this Color color) => 
            color.ApplyOffsets(Global.HueShift, Global.HueOffset, Global.Saturation);

        /// <summary>Applies <see cref="Global"/> hue shift, hue offset and saturation level to each <see cref="Color"/> in this <see cref="System.Collections.IEnumerable"/>.</summary>
        public static Color[] ApplyOffsets(this IEnumerable<Color> colors) => 
            colors.ApplyOffsets(Global.HueShift, Global.HueOffset, Global.Saturation);

        /// <summary>Applies the specified hue shift, hue offset and saturation level to each <see cref="Color"/> in this <see cref="System.Collections.IEnumerable"/>.</summary>
        public static Color[] ApplyOffsets(this IEnumerable<Color> colors, float hueShift, float hueOffset, float saturation) => 
            colors.Select((c, i) => c.ApplyOffsets(hueShift, hueOffset, saturation, i, colors.Count())).ToArray();

        /// <summary>Applies the specified hue shift, hue offset and saturation levels to this <see cref="Color"/>.</summary>
        public static Color ApplyOffsets(this Color color, float hueShift, float hueOffset, float saturation)
        {

            var hsv = color.AsHSV();

            ApplyHueShift(ref hsv, hueShift);
            ApplyHueOffset(ref hsv, hueOffset);
            ApplySaturationLevel(ref hsv, saturation);

            return hsv.AsColor();

        }

        /// <summary>Applies the specified hue shift, hue offset and saturation level to this <see cref="Color"/> that is contained within an <see cref="System.Collections.IEnumerable"/>.</summary>
        public static Color ApplyOffsets(this Color color, float hueShift, float hueOffset, float saturation, int index, int length)
        {

            var hsv = color.AsHSV();

            ApplyHueOffset(ref hsv, hueOffset, index, length);
            ApplyHueShift(ref hsv, hueShift);
            ApplySaturationLevel(ref hsv, saturation);

            return hsv.AsColor();

        }

        /// <summary>Applies an hue shift, specified by normalized value (0-1) across entire color spectrum) to this <see cref="Color"/>.</summary>
        public static void ApplyHueShift(ref (float hue, float saturation, float value) hsv, float offset) =>
            hsv.hue = MathUtility.Wrap(hsv.hue + (offset.Clamp01() * 360), 0, 360);

        /// <summary>Applies an hue offset, specified by 360° value across entire color spectrum) to this <see cref="Color"/>.</summary>
        public static void ApplyHueOffset(ref (float hue, float saturation, float value) hsv, float offset) =>
            hsv.hue = MathUtility.Wrap(hsv.hue + offset, 0, 360);

        /// <summary>Applies an hue offset, specified by 360° value across entire color spectrum) on this <see cref="Color"/> that is contained within an <see cref="System.Collections.IEnumerable"/>.</summary>
        public static void ApplyHueOffset(ref (float hue, float saturation, float value) hsv, float offset, int index, int length)
        {

            var l = (length - 1f).Clamp(2, int.MaxValue);
            offset /= (l);

            hsv.hue = MathUtility.Wrap(hsv.hue + (offset * (index + 1)), 0, 360);

        }

        /// <summary>Applies an saturation level, specified by value 0-1 to this <see cref="Color"/>.</summary>
        public static void ApplySaturationLevel(ref (float hue, float saturation, float value) hsv, float offset) =>
            hsv.saturation = (hsv.saturation.Clamp01() * offset).Clamp01();

        #endregion
        #region Convert from / to HSV

        /// <summary>Converts this RGBA <see cref="Color"/> to HSV.</summary>
        public static (float hue, float saturation, float value) AsHSV(this Color color)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            var HueShift = color.Hue();
            var Saturation = (max == 0) ? 0 : 1f - (1f * min / max);
            var value = max / 255f;

            return (HueShift, Saturation, value);

        }

        /// <summary>Converts HSV color to RGBA <see cref="Color"/>.</summary>
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

        /// <summary>Converts this HSV color to RGBA <see cref="Color"/>.</summary>
        public static Color AsColor(this (float hue, float saturation, float value) hsv) =>
            AsColor(hsv.hue, hsv.saturation, hsv.value);

        #endregion

    }

}
