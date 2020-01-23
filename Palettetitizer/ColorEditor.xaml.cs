using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Palettetitizer
{

    public partial class ColorEditor : UserControl
    {

        public ColorEditor()
        {
            InitializeComponent();
        }

        public static DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color), typeof(Color), typeof(ColorEditor));
        public static DependencyProperty IsEditableProperty = DependencyProperty.Register(nameof(IsEditable), typeof(bool), typeof(ColorEditor));

        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public bool IsEditable
        {
            get => (bool)GetValue(IsEditableProperty);
            set => SetValue(IsEditableProperty, value);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BorderThickness = IsEditable ? new Thickness(1) : new Thickness(0);
        }

        private async void EyeDropper_Checked(object sender, RoutedEventArgs e)
        {

            var wasMousePressed = false;
            var wasEscPressed = false;
            void OnMouseDown(object s, object e) => wasMousePressed = true;
            void OnKeyDown(object s, KeyEventArgs e) { if (e.Key == Key.Escape) wasEscPressed = true; }

            var overlay = new Window
            {
                Topmost = true,
                AllowsTransparency = true,
                Background = new SolidColorBrush(Color.FromArgb(1, 1, 1, 1)),
                WindowStyle = WindowStyle.None,
                WindowState = WindowState.Maximized
            };
            overlay.Show();

            overlay.MouseLeftButtonDown += OnMouseDown;
            overlay.KeyDown += OnKeyDown;

            popup.StaysOpen = true;

            var savedColor = Color;
            while (!(wasMousePressed || wasEscPressed))
            {
                Color = CursorUtility.GetColorUnderCursor();
                await Task.Delay(250);
            }

            if ((sender is ToggleButton t && t.IsMouseOver) || wasEscPressed)
                Color = savedColor;

            popup.StaysOpen = false;
            eyeDropper.IsChecked = false;
            overlay.MouseLeftButtonDown -= OnMouseDown;
            overlay.KeyDown -= OnKeyDown;
            overlay.Close();

        }

    }

}
