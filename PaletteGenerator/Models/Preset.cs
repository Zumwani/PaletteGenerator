using PaletteGenerator.Utilities;
using System.Windows.Media;
using System.Linq;

namespace PaletteGenerator.Models
{

    class Preset
    {

        public static Preset FromCurrent() =>
            new Preset
            {
                ColumnCount = Global.Columns,
                LeftColor = Global.LeftColor,
                RightColor = Global.RightColor,
                GlobalHue = Global.Hue,
                GlobalSaturation = Global.Saturation,
                Rows = Global.Rows.Select(r => new SerializableRow(r)).ToArray(),
            };

        public void SetCurrent()
        {
            Global.Columns.Value = ColumnCount;
            Global.LeftColor.Value = LeftColor;
            Global.RightColor.Value = RightColor;
            Global.Hue.Value = GlobalHue;
            Global.Saturation.Value = GlobalSaturation;
            Global.Rows.Set(Rows.Select(r => r.ToRow()));
        }

        public float GlobalHue          { get; set; }
        public float GlobalSaturation   { get; set; }

        public Color LeftColor          { get; set; }
        public Color RightColor         { get; set; }

        public int ColumnCount          { get; set; }
        public SerializableRow[] Rows   { get; set; }

        public class SerializableRow
        {

            public Color LeftColor              { get; set; }
            public Color CenterColor            { get; set; }
            public Color RightColor             { get; set; }

            public float Hue                    { get; set; }
            public float Saturation             { get; set; }

            public bool UseCustomHue            { get; set; }
            public bool UseCustomSaturation     { get; set; }

            public SerializableRow() { }

            public SerializableRow(Row row)
            {

                LeftColor = row.LeftColor;
                CenterColor = row.CenterColor;
                RightColor = row.RightColor;

                UseCustomHue = row.UseCustomHue;
                UseCustomSaturation = row.UseCustomSaturation;

                Hue = row.Hue;
                Saturation = row.Saturation;

            }

            public Row ToRow()
            {

                var r = new Row()
                {
                    CenterColor = CenterColor
                };

                r.LeftColor = LeftColor;
                r.RightColor = RightColor;

                r.UseCustomHue = UseCustomHue;
                r.UseCustomSaturation = UseCustomSaturation;

                r.Hue = Hue;
                r.Saturation = Saturation;

                return r;

            }

        }

    }

}
