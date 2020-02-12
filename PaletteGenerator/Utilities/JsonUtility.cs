using PaletteGenerator.UI;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace PaletteGenerator.Utilities
{

    /// <summary>Contains utility functions for json serialization.</summary>
    static class JsonUtility<T> where T : new() 
    {

        static JsonUtility()
        {
            options = new JsonSerializerOptions();
            options.Converters.Add(new ConvertColorToHex());
            options.WriteIndented = true;
        }

        static readonly JsonSerializerOptions options;

        /// <summary>Prompts user to pick file to attempt to deserialize into <typeparamref name="T"/>. Returns null if unsuccessful.</summary>
        public static async Task<T> PromptAndLoad() =>  await Load(PromptUtility.Load() ?? string.Empty);

        /// <summary>Prompts user to pick file that we'll attempt to deserialize <typeparamref name="T"/> into.</summary>
        public static void PromptAndSave(T obj)     => Save(obj, PromptUtility.Save() ?? string.Empty);

        /// <summary>Attempts to deserialize <typeparamref name="T"/> from the specified file. Returns null or a new instance if unsuccessful.</summary>
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
                return JsonSerializer.Deserialize<T>(await File.ReadAllTextAsync(path), options);
            }
            catch (Exception e)
            {
                ErrorMessage(e, "reading");
            }

            return default;

        }

        /// <summary>Attempts to serialize <typeparamref name="T"/> to the specified file.</summary>
        public static Task Save(T obj, string path) =>
            Task.Run(() =>
            {

                if (path == string.Empty || DesignModeUtility.IsInDesignMode)
                    return;

                try
                {
                    File.WriteAllText(path, JsonSerializer.Serialize(obj, options));
                }
                catch (Exception e)
                {
                    ErrorMessage(e, "writing");
                }

            });

        static void ErrorMessage(Exception e, string action) =>
            MessageBox.Show(e.GetType().Name + ":" + Environment.NewLine + Environment.NewLine + e.Message + Environment.NewLine + Environment.NewLine, $"An error occured while {action} json file.");

        /// <summary>Json converter to make json serializer serialize color as hex.</summary>
        public class ConvertColorToHex : System.Text.Json.Serialization.JsonConverter<Color>
        {

            public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
                (Color)ColorConverter.ConvertFromString(reader.GetString());

            public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options) =>
                writer.WriteStringValue((new ColorConverter()).ConvertToString(value));

        }

    }

}
