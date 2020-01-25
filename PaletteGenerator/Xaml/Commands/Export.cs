using System;
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

        public void Execute(object parameter)
        {

            //TODO: Fix export
            //Prompt for file path
            //Serialize parameter row as png

            MessageBox.Show("Not implemented yet.");

        }
        
    }

}
