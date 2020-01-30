using System;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Markup;

namespace PaletteGenerator.Commands
{

    public class OpenUri : MarkupExtension, ICommand
    {

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;
        public override object ProvideValue(IServiceProvider serviceProvider) => this;

        public void Execute(object parameter)
        {
            if (parameter is Uri u || (parameter is string s && Uri.TryCreate(s, UriKind.Absolute, out u)))
             Process.Start("explorer.exe", u.ToString());
        }

    }

}
