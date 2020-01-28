using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace PaletteGenerator
{

    public static class SliderUtility
    {

        public static bool GetMoveToPointOnDrag(DependencyObject obj) => (bool)obj.GetValue(MoveToPointOnDragProperty);
        public static void SetMoveToPointOnDrag(DependencyObject obj, bool value) => obj.SetValue(MoveToPointOnDragProperty, value);

        public static readonly DependencyProperty MoveToPointOnDragProperty = DependencyProperty.RegisterAttached("MoveToPointOnDrag", typeof(bool), typeof(SliderUtility), new PropertyMetadata
        {
            PropertyChangedCallback = (obj, changeEvent) =>
            {
                var slider = (Slider)obj;
                if ((bool)changeEvent.NewValue)
                    slider.MouseMove += async (obj2, mouseEvent) =>
                    {

                        if (mouseEvent.LeftButton == MouseButtonState.Pressed)
                        {

                            slider.RaiseEvent(new MouseButtonEventArgs(mouseEvent.MouseDevice, mouseEvent.Timestamp, MouseButton.Left)
                            {
                                RoutedEvent = UIElement.PreviewMouseLeftButtonDownEvent,
                                Source = mouseEvent.Source,
                            });

                            while (Mouse.LeftButton == MouseButtonState.Pressed)
                                await Task.Delay(100);

                            if (!slider.IsMouseOver)
                                return;

                            if (slider.ToolTip is ToolTip t)
                                t.IsOpen = false;

                            slider.RaiseEvent(new DragCompletedEventArgs(float.NaN, float.NaN, false) { Source = slider });

                        }

                    };
            }
        });

        public static bool GetAllowDragCompletedEventWhileOutOfBounds(DependencyObject obj) => (bool)obj.GetValue(AllowDragCompletedEventWhileOutOfBoundsProperty);
        public static void SetAllowDragCompletedEventWhileOutOfBounds(DependencyObject obj, bool value) => obj.SetValue(AllowDragCompletedEventWhileOutOfBoundsProperty, value);

        public static readonly DependencyProperty AllowDragCompletedEventWhileOutOfBoundsProperty = DependencyProperty.RegisterAttached("AllowDragCompletedEventWhileOutOfBounds", typeof(bool), typeof(SliderUtility), new PropertyMetadata
        {
            PropertyChangedCallback = (obj, changeEvent) =>
            {
                var slider = (Slider)obj;
                if ((bool)changeEvent.NewValue)
                    slider.PreviewMouseLeftButtonDown += async (obj2, mouseEvent) =>
                    {

                        while (Mouse.LeftButton == MouseButtonState.Pressed)
                            await Task.Delay(100);

                        //Check whatever we're actually out of bounds
                        if (slider.IsMouseOver)
                            return;

                        if (slider.ToolTip is ToolTip t)
                            t.IsOpen = false;

                        slider.RaiseEvent(new DragCompletedEventArgs(float.NaN, float.NaN, false) { Source = slider });

                    };
            }
        });

    }

}
