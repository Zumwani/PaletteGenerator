using PaletteGenerator.Models;
using PaletteGenerator.Utilities;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace PaletteGenerator.Commands
{

    class Export : MarkupExtension, ICommand
    {

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) => true;
        public override object ProvideValue(IServiceProvider serviceProvider) => this;

        public void Execute(object parameter)
        {

            var rows = Global.Rows;
            var colors = rows.Select(r => r.AllColors).ToArray();

            LoadingUtility.ShowLoadingScreen(async () =>
            {

                if (PromptUtility.Save(Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Palette Generator")).FullName, "png") is string path)
                {

                    try
                    {
                        
                        var bitmap = colors.AsPNGPalette(Settings.ExportCellSize);

                        using var ms = bitmap.AsBytes();
                        await File.WriteAllBytesAsync(path, ms.ToArray());

                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.GetType().Name + ":" + Environment.NewLine + Environment.NewLine + e.Message + Environment.NewLine + Environment.NewLine, "An error occured when exporting as png.");
                    }

                }

            });

        }

    }

}
