using FontAwesome.WPF;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Windows.Media;
using System.Security.Permissions;
using System.Windows.Media.Imaging;
using System.Windows;
using System.IO;
using Microsoft.Win32.SafeHandles;
using System.Windows.Interop;
using Image = System.Windows.Controls.Image;
using Point = System.Windows.Point;
using point = System.Drawing.Point;
using Size = System.Windows.Size;

namespace PaletteGenerator
{

    internal static class CursorUtility
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

        public static Cursor AsCursor(this FontAwesomeIcon icon)
        {

            var image = new Image
            {
                Width = 32,
                Height = 32
            };

            Awesome.SetContent(image, icon);
            return CreateCursor(image);

        }

        public static Point GetScreenPosition()
        {

            point point = default;
            GetCursorPos(ref point);

            return new Point(point.X, point.Y);

        }

        #region Cursor pinvoke

        private static class NativeMethods
        {

            public struct IconInfo
            {
                public bool fIcon;
                public int xHotspot;
                public int yHotspot;
                public IntPtr hbmMask;
                public IntPtr hbmColor;
            }

            [DllImport("user32.dll")]
            public static extern SafeIconHandle CreateIconIndirect(ref IconInfo icon);

            [DllImport("user32.dll")]
            public static extern bool DestroyIcon(IntPtr hIcon);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo pIconInfo);

        }

        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        private class SafeIconHandle : SafeHandleZeroOrMinusOneIsInvalid
        {

            public SafeIconHandle() : base(true) { }

            override protected bool ReleaseHandle() =>
                NativeMethods.DestroyIcon(handle);
        
        }

        private static Cursor InternalCreateCursor(System.Drawing.Bitmap bmp)
        {

            var iconInfo = new NativeMethods.IconInfo();
            NativeMethods.GetIconInfo(bmp.GetHicon(), ref iconInfo);

            iconInfo.xHotspot = 0;
            iconInfo.yHotspot = 0;
            iconInfo.fIcon = false;

            SafeIconHandle cursorHandle = NativeMethods.CreateIconIndirect(ref iconInfo);
            return CursorInteropHelper.Create(cursorHandle);

        }

        static Cursor CreateCursor(UIElement element)
        {

            element.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            element.Arrange(new Rect(new Point(), element.DesiredSize));

            var rtb =
                new RenderTargetBitmap(
                    (int)element.DesiredSize.Width,
                    (int)element.DesiredSize.Height,
                    96, 96, PixelFormats.Pbgra32);

            rtb.Render(element);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));

            using var ms = new MemoryStream();
            encoder.Save(ms);
            using var bmp = new Bitmap(ms);
            return InternalCreateCursor(bmp);

        }

        #endregion

    }

}
