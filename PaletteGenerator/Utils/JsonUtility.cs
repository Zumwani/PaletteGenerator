using PaletteGenerator.UI;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace PaletteGenerator
{

    public static class JsonUtility<T> where T : new() 
    {

        public static async Task<T> PromptAndLoad() =>  await Load(PromptUtility.Load() ?? string.Empty);
        public static void PromptAndSave(T obj)     => Save(obj, PromptUtility.Save() ?? string.Empty);

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
                    File.WriteAllText(path, JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true }));
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
