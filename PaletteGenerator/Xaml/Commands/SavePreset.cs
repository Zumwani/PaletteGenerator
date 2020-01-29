using System;
using System.Windows.Input;
using System.Windows.Markup;

namespace PaletteGenerator.Commands
{

    public class SavePreset : MarkupExtension, ICommand
    {

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) => true;
        public override object ProvideValue(IServiceProvider serviceProvider) => this;

        public void Execute(object parameter)
        {
            var preset = Preset.FromCurrent();
            LoadingUtility.ShowLoadingScreen(() => JsonObject<Preset>.PromptAndSave(preset));
        }

    }

}
