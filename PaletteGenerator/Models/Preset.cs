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
                GlobalHueShift = Global.HueShift,
                GlobalSaturation = Global.Saturation,
                Rows = Global.Rows.Select(r => new SerializableRow(r)).ToArray(),
            };

        public void SetCurrent()
        {
            Global.Columns.Value = ColumnCount;
            Global.LeftColor.Value = LeftColor;
            Global.RightColor.Value = RightColor;
            Global.HueShift.Value = GlobalHueShift;
            Global.Saturation.Value = GlobalSaturation;
            Global.Rows.Set(Rows.Select(r => r.ToRow()));
        }

        public float GlobalHueShift          { get; set; }
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

            public float HueShift                    { get; set; }
            public float Saturation             { get; set; }

            public bool UseCustomHueShift            { get; set; }
            public bool UseCustomSaturation     { get; set; }

            public SerializableRow() { }

            public SerializableRow(Row row)
            {

                LeftColor = row.LeftColor;
                CenterColor = row.CenterColor;
                RightColor = row.RightColor;

                UseCustomHueShift = row.UseCustomHueShift;
                UseCustomSaturation = row.UseCustomSaturation;

                HueShift = row.HueShift;
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

                r.UseCustomHueShift = UseCustomHueShift;
                r.UseCustomSaturation = UseCustomSaturation;

                r.HueShift = HueShift;
                r.Saturation = Saturation;

                return r;

            }

        }

    }

}
