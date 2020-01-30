using System;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Markup;

namespace PaletteGenerator.Commands
{

    public class CheckForUpdates : MarkupExtension, ICommand
    {

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;
        public override object ProvideValue(IServiceProvider serviceProvider) => this;

        public void Execute(object parameter) =>
             Process.Start("explorer.exe", "https://github.com/Zumwani/PaletteGenerator");

    }

}
