using System;
using System.IO;
using System.Windows;

namespace PaletteGenerator
{

    public partial class App : Application
    {

        public static string DataFolder(string subfolder = "", string subfile = "") =>
            Path.Combine(Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Palette Generator", subfolder)).FullName, subfile);
    
    }

}
