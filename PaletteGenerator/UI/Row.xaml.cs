using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace PaletteGenerator.UI
{

    public partial class RowTemplate
    {

        public void Remove(object sender, RoutedEventArgs e) =>
            MainWindow.Remove((sender as Button)?.DataContext as Row);

        private void CenterColorChanged(object sender, System.EventArgs e)
        {
            if (sender is ColorEditor c && c.DataContext is Row row)
                MainWindow.Recalculate(row);
        }

        private void ToggleButton_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton toggle && toggle.TemplatedParent is ColorPickerLib.Controls.ColorPicker picker)
            {

                picker.IsOpen = true;
            
            }
        }

    }

}
