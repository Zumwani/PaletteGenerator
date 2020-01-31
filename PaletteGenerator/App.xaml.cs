using System.Diagnostics;
using System.Windows.Threading;

namespace PaletteGenerator
{

    public partial class App
    {

        public new static Dispatcher Dispatcher { get; } = Dispatcher.CurrentDispatcher;
        public static Window Window { get; } = Current?.FindResource(nameof(Window)) as Window;

        private async void Application_Startup(object sender, System.Windows.StartupEventArgs e)
        {

        //    PresentationTraceSources.Refresh();
        //    PresentationTraceSources.DataBindingSource.Listeners.Add(new ConsoleTraceListener());
        //    PresentationTraceSources.DataBindingSource.Listeners.Add(new DebugTraceListener());
        //    PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Warning | SourceLevels.Error;

            await Settings.Load();
            Window.Show();

        }

    }

    public class DebugTraceListener : TraceListener
    {
        public override void Write(string message) { }
        public override void WriteLine(string message) => Debugger.Break();
    }

}