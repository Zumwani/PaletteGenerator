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
                Hue = App.Window.Hue,
                Saturation = App.Window.Saturation,
                LeftColor = App.Window.RightColor,
                RightColor = App.Window.RightColor,
                Rows = App.Window.Rows.Select(r => new Row(r)).ToArray(),
            };

        public void SetCurrent()
        {
            App.Window.Hue = Hue;
            App.Window.Saturation = Saturation;
            App.Window.Columns = ColumnCount;
            App.Window.Rows.Set(Rows.Select(r => r.ToRow()));
        }

        public float Hue            { get; set; }
        public float Saturation     { get; set; }

        public Color LeftColor      { get; set; }
        public Color RightColor     { get; set; }

        public int ColumnCount      { get; set; }
        public Row[] Rows           { get; set; }

        public class Row
        {

            public Color Left { get; set; }
            public Color Right { get; set; }

            public float Hue { get; set; }
            public float Saturation { get; set; }

            public Color[] LeftSide { get; set; }
            public Color[] RightSide { get; set; }

            public Row(PaletteGenerator.Row row)
            {

                Left = row.LeftColor;
                Right = row.RightColor;

                Hue = row.CustomHue;
                Saturation = row.CustomSaturation;

                LeftSide = row.LeftSide.ToArray();
                RightSide = row.RightSide.ToArray();

            }

            public PaletteGenerator.Row ToRow()
            {

                var r = new PaletteGenerator.Row()
                {
                    LeftColor = Left,
                    RightColor = Right,
                    CustomHue = Hue,
                    CustomSaturation = Saturation,
                };

                r.LeftSide.Set(LeftSide);
                r.RightSide.Set(RightSide);
                
                return r;

            }

        }

    }

}
