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
                LeftColor = App.Window.LeftColor,
                RightColor = App.Window.RightColor,
                GlobalHue = App.Window.Hue,
                GlobalSaturation = App.Window.Saturation,
                Rows = App.Window.Rows.Select(r => new Row(r)).ToArray(),
            };

        public void SetCurrent()
        {
            App.Window.Columns = ColumnCount;
            App.Window.LeftColor = LeftColor;
            App.Window.RightColor = RightColor;
            App.Window.Hue = GlobalHue;
            App.Window.Saturation = GlobalSaturation;
            App.Window.Rows.Set(Rows.Select(r => r.ToRow()));
        }

        public float GlobalHue          { get; set; }
        public float GlobalSaturation   { get; set; }

        public Color LeftColor          { get; set; }
        public Color RightColor         { get; set; }

        public int ColumnCount          { get; set; }
        public Row[] Rows               { get; set; }

        public class Row
        {

            public CustomValue<Color> LeftColor         { get; set; }
            public Color CenterColor                    { get; set; }
            public CustomValue<Color> RightColor        { get; set; }

            public CustomValue<float> Hue         { get; set; }
            public CustomValue<float> Saturation  { get; set; }

            public Row() { }

            public Row(PaletteGenerator.Row row)
            {

                LeftColor = row.LeftColor;
                CenterColor = row.CenterColor;
                RightColor = row.RightColor;

                Hue = row.Hue;
                Saturation = row.Saturation;

            }

            public PaletteGenerator.Row ToRow()
            {

                var r = new PaletteGenerator.Row()
                {
                    CenterColor = CenterColor
                };

                r.LeftColor.Set(LeftColor);
                r.RightColor.Set(RightColor);
                r.Hue.Set(Hue);
                r.Saturation.Set(Saturation);

                return r;

            }

        }

    }

}
