using System.Windows;
using System.Windows.Data;

namespace PaletteGenerator.Utilities
{

    /// <summary>Contains attached properties for binding width to height / height to width.</summary>
    static class SizeUtility
    {

        public static bool GetWidthToHeight(FrameworkElement obj) => (bool)obj.GetValue(WidthToHeightProperty);
        public static void SetWidthToHeight(FrameworkElement obj, bool value) => obj.SetValue(WidthToHeightProperty, value);

        public static readonly DependencyProperty WidthToHeightProperty = DependencyProperty.RegisterAttached("WidthToHeight", typeof(bool), typeof(SliderUtility), new PropertyMetadata
        {
            PropertyChangedCallback = (obj, changeEvent) =>
            {

                if (!(obj is FrameworkElement element))
                    return;

                if (changeEvent.NewValue is bool b)
                    element.SetBinding(FrameworkElement.WidthProperty, new Binding() { Source = element, Path = new PropertyPath(nameof(FrameworkElement.Height)) });

            }

        });

        public static bool GetHeightToWidth(FrameworkElement obj) => (bool)obj.GetValue(HeightToWidthProperty);
        public static void SetHeightToWidth(FrameworkElement obj, bool value) => obj.SetValue(HeightToWidthProperty, value);

        public static readonly DependencyProperty HeightToWidthProperty = DependencyProperty.RegisterAttached("HeightToWidth", typeof(bool), typeof(SliderUtility), new PropertyMetadata
        {
            PropertyChangedCallback = (obj, changeEvent) =>
            {

                if (!(obj is FrameworkElement element))
                    return;

                if (changeEvent.NewValue is bool b)
                    element.SetBinding(FrameworkElement.HeightProperty, new Binding() { Source = element, Path = new PropertyPath(nameof(FrameworkElement.Width)) });

            }

        });

    }

}
