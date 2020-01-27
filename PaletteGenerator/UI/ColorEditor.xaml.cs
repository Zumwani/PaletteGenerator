using System;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using FontAwesome.WPF;
using ColorPickerLib.Controls;
using System.ComponentModel;

namespace PaletteGenerator.UI
{

    public partial class ColorEditor : UserControl
    {

        static ColorEditor()
        {
            //ColorCanvas.DisplayColorSpaceProperty.OverrideMetadata(typeof(ColorEditor), new PropertyMetadata(ColorSpace.RGB, OnDisplayColorChanged));
        }

        public ColorEditor()
        {
            //DependencyPropertyDescriptor.FromProperty(ColorCanvas.DisplayColorSpaceProperty, typeof(ColorCanvas)).AddValueChanged(freehand, OnDisplayColorChanged);
            InitializeComponent();
        }

        static void OnDisplayColorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {

        }

        public static DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color), typeof(Color), typeof(ColorEditor), new PropertyMetadata(Colors.White, OnColorChanged));
        public static DependencyProperty IsEditableProperty = DependencyProperty.Register(nameof(IsEditable), typeof(bool), typeof(ColorEditor));

        public async Task Show()
        {
            BeginStoryboard((Storyboard)FindResource("ShowAnimation"));
            await Task.Delay(100);
        }

        public async Task Hide()
        {
            BeginStoryboard((Storyboard)FindResource("HideAnimation"));
            await Task.Delay(100);
        }

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

        public bool IsEditable
        {
            get => (bool)GetValue(IsEditableProperty);
            set => SetValue(IsEditableProperty, value);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        public event EventHandler ColorChanged;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0062:Make local function 'static'", Justification = "It cant...")]
        private async void EyeDropper_Checked(object sender, RoutedEventArgs e)
        {

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

            if ((sender is ToggleButton t && t.IsMouseOver) || wasEscPressed)
                Color = savedColor;

            overlay.Open(Deinitalize);

        }

    }

}
