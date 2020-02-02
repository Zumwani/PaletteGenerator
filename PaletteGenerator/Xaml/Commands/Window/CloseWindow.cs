using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace PaletteGenerator.Commands
{

    class CloseWindow : MarkupExtension, ICommand
    {

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) => true;
        public override object ProvideValue(IServiceProvider serviceProvider) => this;

        public void Execute(object parameter) =>
            (parameter as Window)?.Close();

    }

}
