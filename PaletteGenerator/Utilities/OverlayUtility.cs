using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using window = System.Windows.Window;

namespace PaletteGenerator.Utilities
{

    /// <summary>A helper class used to display an overlay on each screen.</summary>
    static class OverlayUtility
    {

        static window[] windows;
        static Action<window> deinitialize;

        public static void Open(Action<window> initialize, Action<window> deinitialize)
        {

            if (windows?.Length == 0)
                Close();

            windows = OpenShell.Screens.Screen.All().Select(GetWindow).ToArray();

            if (initialize != null) 
                windows.ForEach(initialize);

            windows.ForEach(w => w.Show());
            OverlayUtility.deinitialize = deinitialize;

        }

        public static void Close()
        {

            if (deinitialize != null)
                windows.ForEach(deinitialize);

            windows.ForEach(w => { if (w.IsLoaded) w.Close(); });
            windows = Array.Empty<window>();

        }

        static window GetWindow(OpenShell.Screens.Screen screen)
        {

            var window = new window
            {
                Topmost = true,
                AllowsTransparency = true,
                Background = new SolidColorBrush(Color.FromArgb(1, 1, 1, 1)),
                WindowStyle = WindowStyle.None,
                Left = screen.Bounds.Left,
                Top = screen.Bounds.Top,
                Width = screen.Bounds.Width,
                Height = screen.Bounds.Height,
                ShowInTaskbar = false,
                ShowActivated = false,
            };

            return window;

        }

    }

}
