using System.Windows.Media;

namespace PaletteGenerator
{

    public static class ColorUtilty
    {

        public static Color Blend(this Color color, Color backColor, float amount)
        {
            //TODO: Does alpha work like it should?
            byte a = (byte)((color.A * amount) + backColor.A * (1 - amount));
            byte r = (byte)((color.R * amount) + backColor.R * (1 - amount));
            byte g = (byte)((color.G * amount) + backColor.G * (1 - amount));
            byte b = (byte)((color.B * amount) + backColor.B * (1 - amount));
            return Color.FromArgb(a, r, g, b);
        }

    }

}
