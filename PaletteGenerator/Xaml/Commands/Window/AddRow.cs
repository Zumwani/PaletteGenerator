using PaletteGenerator.Models;
using PaletteGenerator.Utilities;
using System;
using System.Windows.Input;
using System.Windows.Markup;

namespace PaletteGenerator.Commands
{

    class AddRow : MarkupExtension, ICommand
    {

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) => true;
        public override object ProvideValue(IServiceProvider serviceProvider) => this;

        public void Execute(object parameter) => Execute();
        public static void Execute() => Global.Rows.Create(Global.MaxRows);

    }

}
