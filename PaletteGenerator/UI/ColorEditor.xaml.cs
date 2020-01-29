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

        public static void RefreshAll()
        {
            controls.ForEach(c => c.PropertyChanged?.Invoke(c, new PropertyChangedEventArgs(nameof(DisplayColor))));
        }

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0062:Make local function 'static'", Justification = "It cant...")]
        private async void EyeDropper_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (!(sender is ToggleButton toggle))
                return;

            e.Handled = true;

            var wasMousePressed = false;
            var wasEscPressed = false;
            void OnMouseDown(object s, object e) => wasMousePressed = true;
            void OnKeyDown(object s, KeyEventArgs e) { if (e.Key == Key.Escape) wasEscPressed = true; }

            void Initalize(Window w)
            {
                w.Cursor = FontAwesomeIcon.Eyedropper.AsCursor();
                w.MouseLeftButtonDown += OnMouseDown;
                w.KeyDown += OnKeyDown;
            }

            void Deinitalize(Window w)
            {
                w.MouseLeftButtonDown -= OnMouseDown;
                w.KeyDown -= OnKeyDown;
            }

            var overlay = new OverlayUtility.Overlay();
            overlay.Open(Initalize);

            var savedColor = Color;
            while (!(wasMousePressed || wasEscPressed))
            {
                Color = CursorUtility.GetColorUnderCursor();
                await Task.Delay(100);
            }

            if (toggle.IsMouseOver || wasEscPressed)
                Color = savedColor;

            overlay.Close(Deinitalize);
            toggle.IsChecked = false;

        }

    }

}
