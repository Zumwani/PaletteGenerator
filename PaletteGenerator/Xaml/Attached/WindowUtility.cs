using System.Windows;
using System.Windows.Controls;

namespace PaletteGenerator.UI
{

    public static class WindowUtility
    {

        public static bool GetIsMaximizeButton(Button obj) => (bool)obj.GetValue(IsMaximizeButtonProperty);
        public static void SetIsMaximizeButton(Button obj, bool value) => obj.SetValue(IsMaximizeButtonProperty, value);

        public static readonly DependencyProperty IsMaximizeButtonProperty = DependencyProperty.RegisterAttached("IsMaximizeButton", typeof(bool), typeof(SliderUtility), new PropertyMetadata
        {
            PropertyChangedCallback = (obj, e) =>
            {

                if (!(obj is Button button) || DesignModeUtility.IsInDesignMode)
                    return;

                var window = Window.GetWindow(button);

                window.SizeChanged += (s, e1) =>
                {
                    if (window.WindowState == WindowState.Maximized)
                        button.Content = "";
                    else if (window.WindowState == WindowState.Normal)
                        button.Content = "";
                };

            }
        });

    }

}
