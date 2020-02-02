using PaletteGenerator.Models;
using PaletteGenerator.Utilities;
using System;
using System.Windows.Input;
using System.Windows.Markup;

namespace PaletteGenerator.Commands
{

    class SavePreset : MarkupExtension, ICommand
    {

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) => true;
        public override object ProvideValue(IServiceProvider serviceProvider) => this;

        public void Execute(object parameter)
        {
            var preset = Preset.FromCurrent();
            LoadingUtility.ShowLoadingScreen(() => JsonUtility<Preset>.PromptAndSave(preset));
        }

    }

}
