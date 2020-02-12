using PaletteGenerator.Utilities;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PaletteGenerator.UI
{

    /// <summary>Contains attached properties for maximize button.</summary>
    static class WindowUtility
    {

        public static bool GetIsMaximizeButton(Button obj) => (bool)obj.GetValue(IsMaximizeButtonProperty);
        public static void SetIsMaximizeButton(Button obj, bool value) => obj.SetValue(IsMaximizeButtonProperty, value);

        public static readonly DependencyProperty IsMaximizeButtonProperty = DependencyProperty.RegisterAttached("IsMaximizeButton", typeof(bool), typeof(SliderUtility), new PropertyMetadata
        {
            PropertyChangedCallback = async (obj, e) =>
            {

                if (!(obj is Button button) || DesignModeUtility.IsInDesignMode)
                    return;

                while (App.Window == null)
                    await Task.Delay(100);

                App.Window.SizeChanged += (s, e1) =>
                {
                    if (App.Window.WindowState == WindowState.Maximized)
                        button.Content = "";
                    else if (App.Window.WindowState == WindowState.Normal)
                        button.Content = "";
                };

            }
        });

    }

}
