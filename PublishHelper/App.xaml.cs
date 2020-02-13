using System.IO;
using System.Linq;
using System.Windows;

namespace PublishHelper
{

    public partial class App : Application
    {

#nullable enable
        public static string? OverridenRoot { get; private set; }
#nullable disable

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (Directory.Exists(e.Args.FirstOrDefault()))
                OverridenRoot = e.Args.FirstOrDefault();
        }

    }

}
