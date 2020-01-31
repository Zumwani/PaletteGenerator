using System.Windows.Threading;

namespace PaletteGenerator
{

    public partial class App
    {

        public new static Dispatcher Dispatcher { get; } = Dispatcher.CurrentDispatcher;
        public static Window Window { get; } = Current?.FindResource(nameof(Window)) as Window;

        private async void Application_Startup(object sender, System.Windows.StartupEventArgs e)
        {
            await Settings.Load();
            Window.Show();
        }

    }

}
