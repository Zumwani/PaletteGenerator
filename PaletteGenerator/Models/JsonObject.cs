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
    
    public static class JsonObject<T> where T : new() 
    {

        static string DefaultFolder =>
            Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Palette Generator")).FullName;

        #region Prompt

        public static string? PromptSave() => Prompt<VistaSaveFileDialog>();
        public static string? PromptLoad() => Prompt<VistaOpenFileDialog>();

        static string? Prompt<T1>() where T1 : VistaFileDialog, new()
        {

            var dialog = new T1
            {
                InitialDirectory = DefaultFolder,
                Filter = "JSON files|*.json|All files|*.*",
                DefaultExt = ".json",
                AddExtension = true,
            };

            return (dialog.ShowDialog() ?? false) ? dialog.FileName : default;

        }
#nullable disable

        public static async Task<T> PromptAndLoad() =>  await Load(PromptLoad() ?? string.Empty);
        public static void PromptAndSave(T obj)     => Save(obj, PromptSave() ?? string.Empty);

        #endregion
        #region Json

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

        #endregion

    }

}
