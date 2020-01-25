using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;

namespace PaletteGenerator
{
    public class Preset : INotifyPropertyChanged
    {

        string name;
        public string Name
        {
            get => name;
            set { name = value; OnPropertyChanged(); }
        }

        string id;
        public string ID => id != string.Empty ? id : (id = Guid.NewGuid().ToString());

        Row[] rows;
        public Row[] Rows
        {
            get => rows;
            set { rows = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public static Task<Preset[]> LoadAll()
        {

            var tasks =
                Directory.GetFiles(App.DataFolder("Presets"), "*.json").
                Select(f => FromDisk(f)).
                Where(preset => preset != null);

           return Task.WhenAll(tasks);

        }

        public Task Save() =>
            Save(this, App.DataFolder("Presets", id + ".json"));

        public static async Task<Preset> FromDisk(string path)
        {
            using var fs = File.OpenRead(path);
            return await JsonSerializer.DeserializeAsync<Preset>(fs);
        }

        public static Task Save(Preset preset, string path)
        {
            using var fs = File.OpenRead(path);
            return JsonSerializer.SerializeAsync(fs, preset);

        }

    }

}
