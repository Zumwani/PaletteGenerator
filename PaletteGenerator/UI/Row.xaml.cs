using System.Windows;
using System.Windows.Controls;

namespace PaletteGenerator.UI
{

    public partial class RowTemplate
    {

        public void Remove(object sender, RoutedEventArgs e) =>
            Window.Remove((sender as Button)?.DataContext as Row);

        private void CenterColorChanged(object sender, System.EventArgs e)
        {
            if (sender is ColorEditor c && c.DataContext is Row row)
                Window.Recalculate(row);
        }

    }

}
