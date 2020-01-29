using System;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using FontAwesome.WPF;
using System.Collections.Generic;
using System.ComponentModel;
using window = System.Windows.Window;
using System.Windows.Shapes;

namespace PaletteGenerator.UI
{

    public partial class ColorEditor : UserControl, INotifyPropertyChanged
    {

        static readonly List<ColorEditor> controls = new List<ColorEditor>();

        public ColorEditor()
        {
            controls.Add(this);
            InitializeComponent();
        }

        ~ColorEditor() =>
            controls.Remove(this);

        public static void RefreshAll() =>
            controls.ForEach(c => c.PropertyChanged?.Invoke(c, new PropertyChangedEventArgs(nameof(DisplayColor))));

        public static DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color), typeof(Color), typeof(ColorEditor), new PropertyMetadata(Colors.White, OnColorChanged));

        static void OnColorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is ColorEditor c)
                c.ColorChanged?.Invoke(sender, EventArgs.Empty);
        }

        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public Color DisplayColor => Color;

        public event EventHandler ColorChanged;
        public event PropertyChangedEventHandler PropertyChanged;

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

            var savedColor = Color;
            while (!(wasMousePressed || wasEscPressed))
            {
                Color = CursorUtility.GetColorUnderCursor();
                await Task.Delay(100);
            }

            if (toggle.IsMouseOver || wasEscPressed)
                Color = savedColor;

            OverlayUtility.Close();
            toggle.IsChecked = false;

        }

    }

}
