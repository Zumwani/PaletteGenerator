using Ookii.Dialogs.Wpf;
using System;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace PaletteGenerator.Commands
{

    public class LoadPreset : MarkupExtension, ICommand
    {

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) => true;
        public override object ProvideValue(IServiceProvider serviceProvider) => this;

        public async void Execute(object parameter)
        {

            var dialog = new VistaOpenFileDialog
            {
                InitialDirectory = Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Palette Generator")).FullName,
                Filter = "JSON files|*.json|All files|*.*",
            };

            if (dialog.ShowDialog() ?? false)
            {

                using var fs = dialog.OpenFile();

                var rows = await JsonSerializer.DeserializeAsync<Row[]>(fs);
                MainWindow.CurrentRows = rows;
                MainWindow.Recalculate();

            }

        }

    }

}
