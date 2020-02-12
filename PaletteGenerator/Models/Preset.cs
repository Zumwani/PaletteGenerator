using PaletteGenerator.Utilities;
using System.Windows.Media;
using System.Linq;

namespace PaletteGenerator.Models
{

    /// <summary>Serializable representation of the current state of <see cref="Window"/>. <see cref="FromCurrent"/> and <see cref="SetCurrent"/> can be used to get or set current state, also note associated <see cref="Commands.SavePreset"/> and <see cref="Commands.LoadPreset"/>.</summary>
    class Preset
    {

        /// <summary>Gets the current state of <see cref="Window"/>.</summary>
        public static Preset FromCurrent() =>
            new Preset
            {
                ColumnCount = Global.Columns,
                LeftColor = Global.LeftColor,
                RightColor = Global.RightColor,
                GlobalHueShift = Global.HueShift,
                GlobalHueOffset = Global.HueOffset,
                GlobalSaturation = Global.Saturation,
                Rows = Global.Rows.Select(r => new SerializableRow(r)).ToArray(),
            };

        /// <summary>Restores this preset to <see cref="Window"/>.</summary>
        public void SetCurrent()
        {
            Global.Columns.Value = ColumnCount;
            Global.LeftColor.Value = LeftColor;
            Global.RightColor.Value = RightColor;
            Global.HueShift.Value = GlobalHueShift;
            Global.HueOffset.Value = GlobalHueOffset;
            Global.Saturation.Value = GlobalSaturation;
            Global.Rows.Set(Rows.Select(r => r.ToRow()));
        }

        public float GlobalHueShift     { get; set; }
        public float GlobalHueOffset    { get; set; }
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

            public float HueShift               { get; set; }
            public float HueOffset              { get; set; }
            public float Saturation             { get; set; }

            public bool UseCustomHueShift       { get; set; }
            public bool UseCustomHueOffset      { get; set; }
            public bool UseCustomSaturation     { get; set; }

            public SerializableRow() { }

            public SerializableRow(Row row)
            {

                LeftColor = row.LeftColor;
                CenterColor = row.CenterColor;
                RightColor = row.RightColor;

                UseCustomHueShift = row.UseCustomHueShift;
                UseCustomHueOffset = row.UseCustomHueOffset;
                UseCustomSaturation = row.UseCustomSaturation;

                HueShift = row.HueShift;
                HueOffset = row.HueOffset;
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
                r.UseCustomHueOffset = UseCustomHueOffset;
                r.UseCustomSaturation = UseCustomSaturation;

                r.HueShift = HueShift;
                r.HueOffset = HueOffset;
                r.Saturation = Saturation;

                return r;

            }

        }

    }

}
