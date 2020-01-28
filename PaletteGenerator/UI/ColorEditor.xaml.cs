﻿using System;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using FontAwesome.WPF;

namespace PaletteGenerator.UI
{

    public partial class ColorEditor : UserControl
    {

        public ColorEditor() =>
            InitializeComponent();

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

        private void editor_MouseEnter(object sender, MouseEventArgs e)
        {
            
            colorPicker.Padding = new Thickness(4,0,4,0);
        }

        private void editor_MouseLeave(object sender, MouseEventArgs e)
        {
            colorPicker.Padding = new Thickness(0);
        }

    }

}
