using Ookii.Dialogs.Wpf;
using PaletteGenerator.UI;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace PaletteGenerator
{
    
    public static class JsonObject<T>
    {

        static string DefaultFolder =>
            Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Palette Generator")).FullName;

        #region Prompt

        public static async Task<T> PromptLoad()
        {

            var dialog = new VistaOpenFileDialog
            {
                InitialDirectory = DefaultFolder,
                Filter = "JSON files|*.json|All files|*.*",
            };

            if (dialog.ShowDialog() ?? false)
                return await Load(dialog.FileName);

            return default;

        }

        public static void PromptSave(T obj)
        {

            var dialog = new VistaSaveFileDialog
            {
                InitialDirectory = DefaultFolder,
                Filter = "JSON files|*.json|All files|*.*",
                DefaultExt = ".json",
                AddExtension = true,
            };

            if (dialog.ShowDialog() ?? false)
                Save(obj, dialog.FileName);

        }

        #endregion
        #region Json

        public static async Task<T> Load(string path, bool createNewIfNotFound = false)
        {

            if (DesignModeUtility.IsInDesignMode)
                return default;

            try
            {
                var t = JsonSerializer.Deserialize<T>(await File.ReadAllTextAsync(path));
                return t;
            }
            catch (Exception e)
            {
                if (createNewIfNotFound && e is FileNotFoundException)
                    return Activator.CreateInstance<T>();
                else
                    MessageBox.Show(e.GetType().Name + ":" + Environment.NewLine + Environment.NewLine + e.Message + Environment.NewLine + Environment.NewLine, "An error occured while writing json file.");
            }

            return default;

        }

        public static void Save(T obj, string path)
        {

            if (DesignModeUtility.IsInDesignMode)
                return;

            LoadingUtility.ShowLoadingScreen(async () =>
            {

                try
                {
                    File.WriteAllText(path, JsonSerializer.Serialize(obj));
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.GetType().Name + ":" + Environment.NewLine + Environment.NewLine + e.Message + Environment.NewLine + Environment.NewLine, "An error occured while writing json file.");
                }

            });

        }

        #endregion

    }

}
