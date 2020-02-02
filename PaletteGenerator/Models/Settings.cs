using ColorPickerLib.Controls;
using PaletteGenerator.Utilities;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace PaletteGenerator.Models
{

    static partial class Settings
    {

        public static Setting<ColorMode> ColorMode => Current?.ColorMode;
        public static Setting<ColorSpace> ColorSpace => Current?.ColorSpace;
        public static Setting<int> ExportCellSize => Current?.ExportCellSize;

        #region Setup

        static SettingsClass Current;

        static string SettingsFile =>
            Path.Combine(Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Palette Generator")).FullName, "settings.json");

        public static void Save() => 
            JsonUtility<SettingsClass>.Save(Current, SettingsFile);

        public static async Task Load() =>
            Current = await JsonUtility<SettingsClass>.Load(SettingsFile, true) ?? new SettingsClass();

        public class Setting<T> : Property<T>
        {

            public Setting() { }
            public Setting(T defaultValue) : base(defaultValue) { }

            protected override void OnPropertyChanged([CallerMemberName] string name = "")
            {
                base.OnPropertyChanged(name);
                if (Current != null)
                    Save();
            }

        }

        class SettingsClass
        {

            public Setting<ColorMode> ColorMode   { get; set; } = new Setting<ColorMode>(ColorPickerLib.Controls.ColorMode.ColorPalette);
            public Setting<ColorSpace> ColorSpace { get; set; } = new Setting<ColorSpace>(ColorPickerLib.Controls.ColorSpace.RGB);
            public Setting<int> ExportCellSize    { get; set; } = new Setting<int>(22);

        }

        #endregion

    }

}
