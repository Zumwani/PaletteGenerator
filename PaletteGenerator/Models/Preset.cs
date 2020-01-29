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
                Left = App.Window.RightColor,
                Right = App.Window.RightColor,
                Rows = App.Window.Rows.Select(r => r.Center).ToArray(),
            };

        public void SetCurrent()
        {
            App.Window.Hue = HueOffset;
            App.Window.Saturation = SaturationOffset;
            App.Window.Columns = ColumnCount;
            App.Window.Rows.Set(Rows.Select(c => new Row() { Center = c }));
            Window.Recalculate();
        }

        public int ColumnCount { get; set; }
        public float HueOffset { get; set; }
        public float SaturationOffset { get; set; }
        public Color Left { get; set; }
        public Color Right { get; set; }
        public Color[] Rows { get; set; }

    }

}
