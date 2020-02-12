using System;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;
using window = System.Windows.Window;
using PaletteGenerator.Utilities;
using PaletteGenerator.Models;
using System.Collections.Generic;

namespace PaletteGenerator.UI
{

    partial class ColorEditor : UserControl, INotifyPropertyChanged
    {

        public ColorEditor()
        {
            InitializeComponent();
            Global.HueShift.PropertyChanged   += (s, e) => UpdateDisplayColor();
            Global.HueOffset.PropertyChanged  += (s, e) => UpdateDisplayColor();
            Global.Saturation.PropertyChanged += (s, e) => UpdateDisplayColor();
        }

        #region Properties

        public static DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color), typeof(Color), typeof(ColorEditor), new PropertyMetadata(Refresh));
        public static DependencyProperty GlobalColorProperty = DependencyProperty.Register(nameof(GlobalColor), typeof(Color), typeof(ColorEditor), new PropertyMetadata(Refresh));
        public static DependencyProperty DisplayColorProperty = DependencyProperty.Register(nameof(DisplayColor), typeof(Color), typeof(ColorEditor));
        public static DependencyProperty HueShiftProperty = DependencyProperty.Register(nameof(HueShift), typeof(float), typeof(ColorEditor), new PropertyMetadata(0f, Refresh));
        public static DependencyProperty SaturationProperty = DependencyProperty.Register(nameof(Saturation), typeof(float), typeof(ColorEditor), new PropertyMetadata(1f, Refresh));
        public static DependencyProperty PopupPlacementProperty = DependencyProperty.Register(nameof(PopupPlacement), typeof(PlacementMode), typeof(ColorEditor), new PropertyMetadata(PlacementMode.Bottom));

        public float HueShift
        {
            get => (float)GetValue(HueShiftProperty);
            set => SetValue(HueShiftProperty, value);
        }

        public float Saturation
        {
            get => (float)GetValue(SaturationProperty);
            set => SetValue(SaturationProperty, value);
        }
        public bool IsGlobalPicker { get; set; }

        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public Color GlobalColor
        {
            get => (Color)GetValue(GlobalColorProperty);
            set => SetValue(GlobalColorProperty, value);
        }

        public Color DisplayColor
        {
            get => (Color)GetValue(DisplayColorProperty);
            set => SetValue(DisplayColorProperty, value);
        }

        public PlacementMode PopupPlacement
        {
            get => (PlacementMode)GetValue(PopupPlacementProperty);
            set => SetValue(PopupPlacementProperty, value);
        }

        public bool IsGlobal => Color == GlobalColor;

        static List<ColorEditor> SupressRefresh = new List<ColorEditor>();
        static void Refresh(DependencyObject s, DependencyPropertyChangedEventArgs e)
        {

            if (!(s is ColorEditor c) || SupressRefresh.Contains(c))
                return;

            if (e.Property == GlobalColorProperty)
            {
                if (c.Color == (Color)e.OldValue)
                    c.Color = (Color)e.NewValue;
            }

            if (e.Property == ColorProperty)
            {
                if (c.IsGlobalPicker)
                    c.GlobalColor = (Color)e.NewValue;
            }

            c.OnPropertyChanged(nameof(IsGlobal));
            c.UpdateDisplayColor();

        }

        public void SetOffsets(float hueShift, float saturation)
        {
            SupressRefresh.Add(this);
            HueShift = hueShift;
            Saturation = saturation;
            SupressRefresh.Remove(this);
            UpdateDisplayColor();
        }

        void UpdateDisplayColor() =>
            DisplayColor = Color.ApplyOffsets(HueShift, 0, Saturation);

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        #endregion

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

        private void ResetColorButton_Click(object sender, RoutedEventArgs e) =>
            Color = GlobalColor;

    }

}
