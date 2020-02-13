using System.Windows;

namespace PaletteGenerator
{
     
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
