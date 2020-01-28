using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;

namespace PaletteGenerator.UI
{

    public partial class SliderTemplate
    {

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            (new Sliders.Commands.ShowToolTip()).Execute(((FrameworkElement)sender).Tag);
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            (new Sliders.Commands.HideTooltip()).Execute(((FrameworkElement)sender).Tag);
        }

    }

    namespace Sliders.Commands
    {

        public abstract class ToolTipCommand : MarkupExtension, ICommand
        {

            #region MarkupExtension, ICommand

            public event EventHandler CanExecuteChanged;
            public bool CanExecute(object parameter) => true;
            public override object ProvideValue(IServiceProvider serviceProvider) => this;

            #endregion

            protected static readonly ToolTip sliderTooltip = new ToolTip();

            public abstract void Execute(object parameter);

            protected static void UpdateTooltip(Slider slider)
            {
                sliderTooltip.Content = slider.Value < 1.1 ? Math.Round(slider.Value * 100) + "%" : slider.Value.ToString();
                var pos = CursorUtility.GetScreenPosition();
                sliderTooltip.HorizontalOffset = pos.X + 22;
                sliderTooltip.VerticalOffset = pos.Y + 22;
                slider.ToolTip = sliderTooltip;
            }

        }

        public class HideTooltip : ToolTipCommand
        {

            public override void Execute(object parameter) =>
                sliderTooltip.IsOpen = false;

        }

        public class ShowToolTip : ToolTipCommand
        {

            public override void Execute(object parameter)
            {

                if (!(parameter is Slider slider))
                    return;

                sliderTooltip.Placement = PlacementMode.AbsolutePoint;
                sliderTooltip.IsOpen = true;
                UpdateTooltip(slider);

            }

        }

    }

}
