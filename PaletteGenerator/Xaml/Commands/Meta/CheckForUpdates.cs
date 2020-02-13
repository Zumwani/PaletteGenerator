using PaletteGenerator.Models;
using System;
using System.Net;
using System.Windows.Input;
using System.Windows.Markup;
using System.Text.Json;
using PaletteGenerator.Utilities;
using System.Windows;
using System.Diagnostics;

namespace PaletteGenerator.Commands
{

    class CheckForUpdates : MarkupExtension, ICommand
    {

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;
        public override object ProvideValue(IServiceProvider serviceProvider) => this;

        public void Execute(object parameter) =>
            LoadingUtility.ShowLoadingScreen(() =>
        {

            try
            {

                using var client = new WebClient();
                var json = client.DownloadString(Global.VersionUri);

                var version = JsonSerializer.Deserialize<Version>(json);

                if (System.Version.Parse(version.Message) > System.Version.Parse(Global.Version))
                    if (MessageBox.Show(
                        $"Version {version.Message} is available (current version: {Global.Version})." +
                        Environment.NewLine + Environment.NewLine +
                        "Do you wish to download the newest version now?", "Update available!", MessageBoxButton.YesNoCancel) == MessageBoxResult.Yes)
                        Process.Start("explorer", Global.Github);

            }
            catch (Exception)
            { }

        });

        public class Version
        {
            public string Message { get; set; }
        }

    }

}
