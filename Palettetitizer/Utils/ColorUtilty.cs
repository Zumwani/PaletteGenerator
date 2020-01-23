namespace Palettetitizer
{
    public static class ColorUtilty
    {

        public static System.Drawing.Color ToDrawing(this System.Windows.Media.Color color) =>
            System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);

        public static System.Windows.Media.Color ToWPF(this System.Drawing.Color color) =>
            System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);

    }

}
