using System;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;
using window = System.Windows.Window;

namespace PaletteGenerator.UI
{

    public partial class ColorEditor : UserControl, INotifyPropertyChanged
    {

        public ColorEditor() => InitializeComponent();

        public static DependencyProperty GlobalColorProperty = DependencyProperty.Register(nameof(GlobalColor), typeof(Color), typeof(ColorEditor), new PropertyMetadata(OnColorChanged));
        public static DependencyProperty CustomColorProperty = DependencyProperty.Register(nameof(CustomColor), typeof(Color), typeof(ColorEditor), new PropertyMetadata(OnColorChanged));
        public static DependencyProperty SelectedColorProperty = DependencyProperty.Register(nameof(SelectedColor), typeof(Color), typeof(ColorEditor), new PropertyMetadata(Colors.LightSkyBlue));

        static void OnColorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {

            if (!(sender is ColorEditor c))
                return;

            if (e.Property == GlobalColorProperty && c.CustomColor == (Color)e.OldValue)
                c.CustomColor = c.GlobalColor;

            Task.Run(async () =>
            {

                await Task.Delay(100);

                App.Dispatcher.Invoke(() =>
                {

                    c.OnPropertyChanged(nameof(HasCustomColor));
                    c.SetValue(SelectedColorProperty, c.HasCustomColor ? c.CustomColor : c.GlobalColor);

                });

            }).ConfigureAwait(false);

        }

        public Color GlobalColor
        {
            get => (Color)GetValue(GlobalColorProperty);
            set => SetValue(GlobalColorProperty, value);
        }

        public Color CustomColor
        {
            get => (Color)GetValue(CustomColorProperty);
            set => SetValue(CustomColorProperty, value);
        }

        public bool HasCustomColor => CustomColor != GlobalColor; 
        public Color SelectedColor
        {
            get => (Color)GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private async void EyeDropper_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (!(sender is ToggleButton toggle))
                return;

            e.Handled = true;

            var wasMousePressed = false;
            var wasEscPressed = false;
            void OnMouseDown(object s, object e) => wasMousePressed = true;
            void OnKeyDown(object s, KeyEventArgs e) { if (e.Key == Key.Escape) wasEscPressed = true; }

            void Initalize(window w)
            {
                Mouse.OverrideCursor = new Cursor(Application.GetResourceStream(new Uri("pack://application:,,,/Assets/color-picker.cur", UriKind.Absolute)).Stream);
                w.MouseLeftButtonDown += OnMouseDown;
                w.KeyDown += OnKeyDown;
            }

            void Deinitalize(window w)
            {
                w.MouseLeftButtonDown -= OnMouseDown;
                w.KeyDown -= OnKeyDown;
                Mouse.OverrideCursor = null;
            }

            OverlayUtility.Open(Initalize, Deinitalize);

            var savedColor = CustomColor;
            while (!(wasMousePressed || wasEscPressed))
            {
                CustomColor = CursorUtility.GetColorUnderCursor();
                await Task.Delay(100);
            }

            if (toggle.IsMouseOver || wasEscPressed)
                CustomColor = savedColor;

            OverlayUtility.Close();
            toggle.IsChecked = false;

        }

        private void ResetColorButton_Click(object sender, RoutedEventArgs e)
        {
            CustomColor = GlobalColor;
        }

    }

}
