using System.Windows;

namespace PaletteGenerator
{

    //TODO: Fix installer (integrate with github and download from release branch (create dev and release branches), perhaps installer should be reuseable as well)
    //TODO: When installer is done, integrate update function through CheckForUpdates command

    partial class Window : System.Windows.Window
    {

        public Window() =>
            InitializeComponent();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Commands.AddRow.Execute();
            Commands.AddRow.Execute();
        }

    }

}
