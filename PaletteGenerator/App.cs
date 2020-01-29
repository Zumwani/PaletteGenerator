using System;
using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace PaletteGenerator
{

    public static class App
    {

        public static Dispatcher Dispatcher { get; } = Dispatcher.CurrentDispatcher;
        public static Window Window { get; } = new Window();

        public static string DataFolder(string subfolder = "", string subfile = "") =>
            Path.Combine(Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Palette Generator", subfolder)).FullName, subfile);

        [STAThread]
        public static void Main(string[] args)
        {
            Settings.Load().Wait();
            Window.ShowDialog();
        }

    }

}
