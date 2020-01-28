using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace PaletteGenerator.Commands
{

    public class ToggleMaximizeWindow : MarkupExtension, ICommand
    {

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) => true;
        public override object ProvideValue(IServiceProvider serviceProvider) => this;

        public void Execute(object parameter)
        {
            if (parameter is Window w)
                if (w.WindowState == WindowState.Normal)
                    w.WindowState = WindowState.Maximized;
                else
                    w.WindowState = WindowState.Normal;
        }

    }

}
