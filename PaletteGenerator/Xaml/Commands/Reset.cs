using PaletteGenerator.Models;
using System;
using System.Windows.Input;
using System.Windows.Markup;

namespace PaletteGenerator.Commands
{
    
    class Reset : MarkupExtension, ICommand
    {

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) => true;
        public override object ProvideValue(IServiceProvider serviceProvider) => this;

        public void Execute(object parameter)
        {

            Global.HueShift.Value = 0;
            Global.HueOffset.Value = 0;
            Global.Saturation.Value = 1;

            var count = Global.Rows.Count;
            Global.Rows.RaiseListChangedEvents = false;
            Global.Rows.Clear();
            for (int i = 0; i < count; i++)
                Global.Rows.Add(new Row());
            Global.Rows.RaiseListChangedEvents = true;
            Global.Rows.ResetBindings();

        }

    }

}
