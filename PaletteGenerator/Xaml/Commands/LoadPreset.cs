using PaletteGenerator.Models;
using PaletteGenerator.Utilities;
using System;
using System.Windows.Input;
using System.Windows.Markup;

namespace PaletteGenerator.Commands
{

    class LoadPreset : MarkupExtension, ICommand
    {

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) => true;
        public override object ProvideValue(IServiceProvider serviceProvider) => this;

        public void Execute(object parameter)
        {

            Preset preset = null;
            LoadingUtility.ShowLoadingScreen(async () => 
            {
                preset = await JsonUtility<Preset>.PromptAndLoad();
                App.Dispatcher.Invoke(() => preset?.SetCurrent());
            });

        }

    }

}
