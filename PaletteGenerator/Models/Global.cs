using System.ComponentModel;
using System.Windows.Media;

namespace PaletteGenerator.Models
{

    /// <summary>Contains global values that does not need to be persisted.</summary>
    static class Global
    {

        public static Property<Color> LeftColor     { get; } = new Property<Color>(Colors.White);
        public static Property<Color> RightColor    { get; } = new Property<Color>(Colors.Black);

        public static Property<float> HueShift      { get; } = new Property<float>(0);
        public static Property<float> HueOffset     { get; } = new Property<float>(0);
        public static Property<float> Saturation    { get; } = new Property<float>(1);

        public static Property<int> Columns         { get; } = new Property<int>(4);
        public static BindingList<Row> Rows         { get; } = new BindingList<Row>();

        public static int MaxRows       => 16;
        public static int MaxColumns    => 16;
        public static int MinColumns    => 2;
        public static string Github     => "https://github.com/Zumwani/PaletteGenerator";
        public static string Discord    => "https://discord.gg/P8VX7Wb";
        public static string Version    => typeof(Window).Assembly.GetName().Version.ToString(2);

    }

}
