using Microsoft.Build.Evaluation;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PublishHelper
{

    static class IncrementVersion
    {

        public static string Increment(string projectPath)
        {
            IncrementProjectVersion(projectPath, out var version);
            SetJsonVersion(version);
            return version;
        }

        public static void IncrementProjectVersion(string path, out string version)
        {

            var xml = File.ReadAllText(path);
            var v = "<Version>";
            var v2 = "</Version>";
            var versionTag = xml.IndexOf(v) + v.Length;
            int versionEndTag() => xml.IndexOf(v2);

            version = xml.Substring(versionTag, versionEndTag() - versionTag);
            
            var projVersion = System.Version.Parse(version);
            version = projVersion.Major + "." + (projVersion.Minor + 1);

            xml = xml.Remove(versionTag, versionEndTag() - versionTag);
            xml = xml.Insert(versionTag, version);

            File.WriteAllText(path, xml);

        }

        static readonly dynamic options = new JsonSerializerOptions() { WriteIndented = true };
        public static void SetJsonVersion(string version)
        {

            var file = Path.Combine(Strings.Root, @"version.json");

            var json =
                File.Exists(file) ?
                JsonSerializer.Deserialize<Version>(File.ReadAllText(file), options) :
                Version.Default;

            json.Message = version;

            File.WriteAllText(file, JsonSerializer.Serialize<Version>(json, options));

        }

    }

    public struct Version
    {

        public static Version Default => new Version() { Label = "Current version:", Color = "lightgray", Style = "flat-square" };

        [JsonPropertyName("label")]
        public string Label { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("color")]
        public string Color { get; set; }

        [JsonPropertyName("style")]
        public string Style { get; set; }

    }

}
