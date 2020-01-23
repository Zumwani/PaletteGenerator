using System.Windows;
using System.Windows.Documents;

namespace Palettetitizer.UI
{

    public partial class RowTemplate
    {

        public void Remove(object sender, RoutedEventArgs e) =>
            MainWindow.Remove((sender as Hyperlink)?.DataContext as Row);

    }

}
