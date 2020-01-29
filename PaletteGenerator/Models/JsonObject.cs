#nullable enable
using Ookii.Dialogs.Wpf;
using PaletteGenerator.UI;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace PaletteGenerator
{
    
    public static class Prompt
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

            return (dialog.ShowDialog() ?? false) ? dialog.FileName : default;

        }
#nullable disable

    }

    public static class JsonObject<T> where T : new() 
    {

        public static async Task<T> PromptAndLoad() =>  await Load(Prompt.Load() ?? string.Empty);
        public static void PromptAndSave(T obj)     => Save(obj, Prompt.Save() ?? string.Empty);

        public static async Task<T> Load(string path, bool createNewIfNotFound = false)
        {

            if (path == string.Empty || DesignModeUtility.IsInDesignMode)
                return default;

            if (!File.Exists(path))
                if (createNewIfNotFound)
                    return new T();
                else
                    ErrorMessage(new FileNotFoundException("", path), "reading");

            try
            {
                return JsonSerializer.Deserialize<T>(await File.ReadAllTextAsync(path));
            }
            catch (Exception e)
            {
                ErrorMessage(e, "reading");
            }

            return default;

        }

        public static Task Save(T obj, string path) =>
            Task.Run(() =>
            {

                if (path == string.Empty || DesignModeUtility.IsInDesignMode)
                    return;

                try
                {
                    File.WriteAllText(path, JsonSerializer.Serialize(obj));
                }
                catch (Exception e)
                {
                    ErrorMessage(e, "writing");
                }

            });

        static void ErrorMessage(Exception e, string action) =>
            MessageBox.Show(e.GetType().Name + ":" + Environment.NewLine + Environment.NewLine + e.Message + Environment.NewLine + Environment.NewLine, $"An error occured while {action} json file.");

    }

}
