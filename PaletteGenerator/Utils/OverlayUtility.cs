using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace PaletteGenerator
{

    /// <summary>A helper class used to display an overlay on each screen.</summary>
    public static class OverlayUtility
    {

        public class Overlay
        {

            public Overlay() =>
                windows = OpenShell.Screens.Screen.All().Select(GetWindow).ToArray();

            readonly Window[] windows;

            public void Open(Action<Window> action)
            {
                windows.ForEach(action);
                windows.ForEach(w => w.Show());
            }

            public void Close(Action<Window> action)
            {
                windows.ForEach(action);
                windows.ForEach(w => { if (w.IsLoaded) w.Close(); });
            }

            Window GetWindow(OpenShell.Screens.Screen screen)
            {

                var window = new Window
                {
                    Topmost = true,
                    AllowsTransparency = true,
                    Background = new SolidColorBrush(Color.FromArgb(1, 1, 1, 1)),
                    WindowStyle = WindowStyle.None,
                    Left = screen.Bounds.Left,
                    Top = screen.Bounds.Top,
                    WindowState = WindowState.Maximized,
                    ShowInTaskbar = false,
                    ShowActivated = false,
                };

                return window;

            }

        }

    }

}
