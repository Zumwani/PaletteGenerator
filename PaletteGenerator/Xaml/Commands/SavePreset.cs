using Ookii.Dialogs.Wpf;
using System;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace PaletteGenerator.Commands
{

    public class SavePreset : MarkupExtension, ICommand
    {

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) => true;
        public override object ProvideValue(IServiceProvider serviceProvider) => this;

        public void Execute(object parameter)
        {

            var dialog = new VistaSaveFileDialog
            {
                InitialDirectory = Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Palette Generator")).FullName,
                Filter = "JSON files|*.json|All files|*.*",
                DefaultExt = ".json",
                AddExtension = true,
            };

            if (dialog.ShowDialog() ?? false)
            {

                var rows = MainWindow.CurrentRows;

                LoadingUtility.ShowLoadingScreen(async () =>
                {

                    try
                    {
                        using var fs = dialog.OpenFile();
                        await JsonSerializer.SerializeAsync(fs, rows);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.GetType().Name + ":" + Environment.NewLine + Environment.NewLine + e.Message + Environment.NewLine + Environment.NewLine, "An error occured while writing json file.");
                    }

                });

            }

        }

    }

}
