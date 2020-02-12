using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Point = System.Windows.Point;
using point = System.Drawing.Point;

namespace PaletteGenerator.Utilities
{

    /// <summary>Contains utility functions for working with the cursor.</summary>
    static class CursorUtility
    {

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(ref point lpPoint);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        private static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        static readonly Bitmap img = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        public static System.Windows.Media.Color GetColorUnderCursor()
        {

            point point = default;
            GetCursorPos(ref point);

            using (Graphics gdest = Graphics.FromImage(img))
            using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero))
                BitBlt(gdest.GetHdc(), 0, 0, 1, 1, gsrc.GetHdc(), point.X, point.Y, (int)CopyPixelOperation.SourceCopy);

            var color = img.GetPixel(0, 0);
            var c = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);

            return c;

        }

        public static Point GetScreenPosition()
        {

            point point = default;
            GetCursorPos(ref point);

            return new Point(point.X, point.Y);

        }

    }

}
