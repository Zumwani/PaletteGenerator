using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace PaletteGenerator.Utils
{

    public static class TransitionUtility
    {

        public static TimeSpan DefaultDuration { get; } = TimeSpan.FromSeconds(0.25); 

        public static T Show<T>(this T element) where T : FrameworkElement
        {
            element.Visibility = Visibility.Visible;
            return element;
        }

        public static T Hide<T>(this T element) where T : FrameworkElement
        {
            element.Visibility = Visibility.Collapsed;
            return element;
        }

        public static Task<T> Fade<T>(this T element, double from, double to, TimeSpan duration = default) where T : FrameworkElement =>
             element.Animate(new DoubleAnimation(from, to, duration), UIElement.OpacityProperty);

        public static Task<T> Fade<T>(this T element, double to, TimeSpan duration = default) where T : FrameworkElement =>
             element.Animate(new DoubleAnimation(to, duration), UIElement.OpacityProperty);

        public static async Task<T> HorizontalSlide<T>(this T element, double from, double to, TimeSpan duration = default) where T : FrameworkElement
        {
            var transform = MakeSureTransformExists<TranslateTransform>(element);
            await transform.Animate(new DoubleAnimation(from, to, duration), TranslateTransform.XProperty);
            return element;
        }

        public static async Task<T> HorizontalSlide<T>(this T element, double to, TimeSpan duration = default) where T : FrameworkElement
        {
            var transform = MakeSureTransformExists<TranslateTransform>(element);
            await transform.Animate(new DoubleAnimation(to, duration), TranslateTransform.XProperty);
            return element;
        }

        public static async Task<T> Animate<T>(this T element, AnimationTimeline animation, DependencyProperty property) where T : IAnimatable
        {

            if (element == null)
                return element;

            if (!animation.Duration.HasTimeSpan || animation.Duration.TimeSpan == TimeSpan.Zero)
                animation.Duration = new Duration(DefaultDuration);

            element.BeginAnimation(property, animation);
            await Task.Delay(animation.Duration.TimeSpan);
            return element;

        }

        public static Thickness Add(this Thickness thickness, Thickness add) =>
            new Thickness(thickness.Left + add.Left, thickness.Top + add.Top, thickness.Right + add.Right, thickness.Bottom + add.Bottom);

        static T MakeSureTransformExists<T>(FrameworkElement element) where T : Transform
        {

            element.RenderTransformOrigin = new Point(0.5, 0.5);
            if (element.RenderTransform is TransformGroup group)
            {

                var current = (T)group.Children.FirstOrDefault(t => t is T);

                if (current == null)
                    group.Children.Add(current = Activator.CreateInstance<T>());

                return current;

            }
            else
            {

                var current = element.RenderTransform;

                group = new TransformGroup();
                element.RenderTransform = group;

                if (current != null)
                    group.Children.Add(current);

                current = Activator.CreateInstance<T>();
                group.Children.Add(current);
                return (T)current;

            }

        }

    }

}
