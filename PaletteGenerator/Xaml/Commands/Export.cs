using Ookii.Dialogs.Wpf;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace PaletteGenerator.Commands
{

    public class Export : MarkupExtension, ICommand
    {

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) => true;
        public override object ProvideValue(IServiceProvider serviceProvider) => this;

        public async void Execute(object parameter)
        {

            var dialog = new VistaSaveFileDialog
            {
                InitialDirectory = Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Palette Generator")).FullName,
                Filter = "PNG files|*.png|All files|*.*",
                DefaultExt = ".png",
                AddExtension = true,
            };

            if (dialog.ShowDialog() ?? false)
            {

                await MainWindow.ShowLoadingOverlay();

                try
                {

                    var colors = MainWindow.CurrentRows.Select(r => r.AllColors).ToArray();
                    var bitmap = colors.AsPNGPalette(64);

                    using var ms = bitmap.AsBytes();
                    await File.WriteAllBytesAsync(dialog.FileName, ms.ToArray());

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.GetType().Name + ":" + Environment.NewLine + Environment.NewLine + e.Message + Environment.NewLine + Environment.NewLine, "An error occured when exporting as png.");
                }

                _ = MainWindow.HideLoadingOverlay().ConfigureAwait(false);

            }

        }

    }

}
