﻿using ColorPickerLib.Controls;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace PaletteGenerator
{

    public class Settings : MarkupExtension
    {

        public Property<ColorMode> ColorMode { get; set; }   = new Property<ColorMode>();
        public Property<ColorSpace> ColorSpace { get; set; } = new Property<ColorSpace>();

        #region Setup

        static string SettingsFile =>
            Path.Combine(Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Palette Generator")).FullName, "settings.json");

        public class Property<T> : INotifyPropertyChanged
        {

            T value;
            public T Value
            {
                get => value;
                set { this.value = value; OnPropertyChanged(); }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            void OnPropertyChanged([CallerMemberName] string name = "")
            {
                Current?.Save();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }

        }
        
        public void Save()              => JsonObject<Settings>.Save(this, SettingsFile);
        public static async Task Load() => Current = await JsonObject<Settings>.Load(SettingsFile, true);

        public static Settings Current { get; private set; }

        public override object ProvideValue(IServiceProvider serviceProvider) =>
            Current;

        #endregion

    }

}
