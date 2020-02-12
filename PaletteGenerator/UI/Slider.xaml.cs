using PaletteGenerator.Utilities;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace PaletteGenerator.UI
{

    partial class SliderTemplate
    {

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.TemplatedParent is Slider s)
                ShowTooltip(s);
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e) =>
            HideTooltip();

        protected static readonly ToolTip sliderTooltip = new ToolTip();

        static void ShowTooltip(Slider slider)
        {
            sliderTooltip.Placement = PlacementMode.AbsolutePoint;
            sliderTooltip.IsOpen = true;
            ToolTipService.SetShowDuration(slider, int.MaxValue);
            UpdateTooltip(slider);
        }

        static void HideTooltip() =>
            sliderTooltip.IsOpen = false;

        static void UpdateTooltip(Slider slider)
        {
            sliderTooltip.Content = slider.Value < 1.1 ? Math.Round(slider.Value * 100) + "%" : slider.Value.ToString();
            var pos = CursorUtility.GetScreenPosition();
            sliderTooltip.HorizontalOffset = pos.X + 22;
            sliderTooltip.VerticalOffset = pos.Y + 22;
            slider.ToolTip = sliderTooltip;
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is FrameworkElement element && element.TemplatedParent is Slider s)
            {
                s.Value += s.SmallChange * (e.Delta > 10 ? 1 : -1);
                UpdateTooltip(s);
            }
        }

    }

}
