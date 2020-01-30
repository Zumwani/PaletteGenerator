using PaletteGenerator.UI;
using System.Linq;
using System.Windows.Media;

namespace PaletteGenerator
{

    public class Preset
    {

        public static Preset FromCurrent() =>
            new Preset
            {
                ColumnCount = App.Window.Columns,
                HueOffset = App.Window.Hue,
                SaturationOffset = App.Window.Saturation,
                LeftColor = App.Window.RightColor,
                RightColor = App.Window.RightColor,
                Rows = App.Window.Rows.Select(r => r.CenterColor).ToArray(),
            };

        public void SetCurrent()
        {
            App.Window.Hue = HueOffset;
            App.Window.Saturation = SaturationOffset;
            App.Window.Columns = ColumnCount;
            App.Window.Rows.Set(Rows.Select(c => new Row() { CenterColor = c }));
        }

        public int ColumnCount { get; set; }
        public float HueOffset { get; set; }
        public float SaturationOffset { get; set; }
        public Color LeftColor { get; set; }
        public Color RightColor { get; set; }
        public Color[] Rows { get; set; }

    }

}
