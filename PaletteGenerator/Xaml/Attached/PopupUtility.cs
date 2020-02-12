using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace PaletteGenerator.Utilities
{

    /// <summary>Contains attached properties for attaching <see cref="Popup"/> to <see cref="ToggleButton"/>.</summary>
    static class PopupUtility
    {

        public static Popup GetAttach(ToggleButton obj) => (Popup)obj.GetValue(AttachProperty);
        public static void SetAttach(ToggleButton obj, Popup value) => obj.SetValue(AttachProperty, value);

        public static readonly DependencyProperty AttachProperty = DependencyProperty.RegisterAttached("Attach", typeof(Popup), typeof(PopupUtility), new PropertyMetadata
        {
            PropertyChangedCallback = async (obj, changeEvent) =>
            {

                if (!(obj is ToggleButton toggle))
                    return;

                if (changeEvent.NewValue is Popup popup)
                {

                    toggle.SetBinding(UIElement.IsHitTestVisibleProperty, new Binding() { Source = popup, Path = new PropertyPath(nameof(Popup.IsOpen)), Converter = new Converters.InvertBool() });
                    popup.SetBinding(Popup.IsOpenProperty, new Binding() { Source = toggle, Path = new PropertyPath(nameof(ToggleButton.IsChecked)) });
                    popup.PlacementTarget = toggle;
                    popup.StaysOpen = false;

                    await Task.Delay(100);

                    var binding = BindingOperations.GetBinding(toggle, UIElement.VisibilityProperty);
                    if (binding != null)
                    {
                        popup.Opened += (s, e) => toggle.Visibility = Visibility.Visible;
                        popup.Closed += (s, e) => BindingOperations.SetBinding(toggle, UIElement.VisibilityProperty, binding);
                    }

                }

            }

        });

        public static PlacementMode GetAttachedPopupPlacement(ToggleButton obj) => (PlacementMode)obj.GetValue(AttachedPopupPlacementProperty);
        public static void SetAttachedPopupPlacement(ToggleButton obj, PlacementMode value) => obj.SetValue(AttachedPopupPlacementProperty, value);

        public static readonly DependencyProperty AttachedPopupPlacementProperty = DependencyProperty.RegisterAttached("AttachedPopupPlacement", typeof(PlacementMode), typeof(PopupUtility), new PropertyMetadata
        {
            PropertyChangedCallback = (obj, changeEvent) =>
            {

                if (!(obj is ToggleButton toggle))
                    return;

                if (changeEvent.NewValue is PlacementMode placement && GetAttach(toggle) is Popup popup)
                    popup.SetValue(Popup.PlacementProperty, placement);

            }

        });

    }

}
