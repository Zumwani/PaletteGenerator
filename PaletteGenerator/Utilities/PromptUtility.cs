#nullable enable
using Ookii.Dialogs.Wpf;
using System;
using System.IO;

namespace PaletteGenerator.Utilities
{

    /// <summary>Contains utility functions for promting the user with save and load dialogs.</summary>
    static class PromptUtility
    {

        static string DefaultFolder =>
            Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Palette Generator")).FullName;

        const string DefaultExt = "json";

        public static string? Save() => DoPrompt<VistaSaveFileDialog>();
        public static string? Load() => DoPrompt<VistaOpenFileDialog>();

        public static string? Save(string initialFolder, string? ext = null) => DoPrompt<VistaSaveFileDialog>(initialFolder, ext);
        public static string? Load(string initialFolder, string? ext = null) => DoPrompt<VistaOpenFileDialog>(initialFolder, ext);

        static string? DoPrompt<T1>(string? initialFolder = null, string? ext = null) where T1 : VistaFileDialog, new()
        {

            initialFolder ??= DefaultFolder;
            ext ??= DefaultExt;

            if (ext.StartsWith("."))
                ext = ext.Remove(0, 1);

            var dialog = new T1
            {
                InitialDirectory = initialFolder,
                Filter = $"{ext} files|*.{ext}|All files|*.*",
                DefaultExt = $".{ext}",
                AddExtension = true,
            };

            return App.Dispatcher.Invoke(() => (dialog.ShowDialog(App.Window) ?? false) ? dialog.FileName : default);

        }
#nullable disable

    }

}
