using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Linq;
using System.ComponentModel;
using System;
using System.Diagnostics;
using PaletteGenerator.Utils;

namespace PaletteGenerator
{

    //TODO: Fix hue offset slider
    //TODO: Fix presets
    //TODO: Look for another color picker or create own
    //TODO: Fix installer (integrate with github and download from release branch (create dev and release branches), perhaps installer should be reuseable as well)
    //TODO: Fix minimize / maximize button, fix (while maximized) drag to normalize
    //TODO: Fix color buttons when hovering at edge

    public partial class MainWindow : Window
    {

        #region Properties

        public static double MaxRows => 16;
        public static double MaxColumns => 16;
        public static double MinColumns => 2;

        public static DependencyProperty ColumnsProperty    = DependencyProperty.Register(nameof(Columns),    typeof(int),   typeof(MainWindow), new PropertyMetadata(4));      //Property change is handled by Slider.MouseUp event, since lag occurs otherwise while dragging slider
        public static DependencyProperty HueProperty        = DependencyProperty.Register(nameof(Hue),        typeof(float), typeof(MainWindow), new PropertyMetadata(0f));   //Property change is handled by Slider.MouseUp event, since lag occurs otherwise while dragging slider
        public static DependencyProperty LeftColorProperty  = DependencyProperty.Register(nameof(LeftColor),  typeof(Color), typeof(MainWindow), new PropertyMetadata(Colors.White, OnColumnsChanged));
        public static DependencyProperty RightColorProperty = DependencyProperty.Register(nameof(RightColor), typeof(Color), typeof(MainWindow), new PropertyMetadata(Colors.Black, OnColumnsChanged));

        static void OnColumnsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) =>
            Recalculate();

        public BindingList<Row> Rows { get; } = new BindingList<Row>();

        public int Columns
        {
            get => (int)GetValue(ColumnsProperty);
            set => SetValue(ColumnsProperty, value);
        }

        public float Hue
        {
            get => (float)GetValue(HueProperty);
            set => SetValue(HueProperty, value);
        }

        public Color LeftColor  
        {
            get => (Color)GetValue(LeftColorProperty); 
            set => SetValue(LeftColorProperty, value); 
        }

        public Color RightColor 
        { 
            get => (Color)GetValue(RightColorProperty); 
            set => SetValue(RightColorProperty, value); 
        }

        #endregion
        #region Rows

        void Add(object sender = null, RoutedEventArgs e = null)
        {
            if (Rows.Count < MaxRows)
                Recalculate(false, Rows.Create());
        }

        public static void Remove(Row row) =>
            Current.Rows.Remove(row);

        public static void Recalculate() =>
            Recalculate(true, Current.Rows.ToArray());

        public static async void Recalculate(bool animate, params Row[] rows)
        {

            if (rows.Length == 0)
                return;

            if (animate)
                await Current.loadingOverlay.Show().Fade(0, 0.5);

            var left = Current.LeftColor;
            var right = Current.RightColor;
            var hue = Current.Hue;

            var steps = Current.Columns / 2 + 1;
            foreach (var row in rows)
            {

                var rowTemplate = (FrameworkElement)Current.list.ItemContainerGenerator.ContainerFromItem(row);
                if (animate)
                    await rowTemplate.Fade(1, 0);

                row.SetColors(await row.Calculate(left, right, steps, hue));
                if (animate)
                    await rowTemplate.Fade(0, 1);
            
            }

            if (animate)
                (await Current.loadingOverlay.Fade(0, 0.5)).Hide();

        }

        #endregion
        #region Window

        public static MainWindow Current { get; private set; }

        public MainWindow()
        {
            Current = this;
            InitializeComponent();
            Unloaded += (s,e) => { if (Current == this) Current = null; };
            Add(); Add();
        }

        void Minimize(object sender, RoutedEventArgs e) =>
            WindowState = WindowState.Minimized;

        void Close(object sender, RoutedEventArgs e) =>
            Close();

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) =>
            DragMove();

        void Update(object sender, RoutedEventArgs e) =>
            Process.Start("explorer.exe", "https://github.com/Zumwani/PaletteGenerator");

        #endregion
        #region Sliders

        private void Slider_KeyUp(object sender, KeyEventArgs e) =>
            Recalculate();

        readonly ToolTip sliderTooltip = new ToolTip();

        async void ShowTooltip(Slider slider)
        {

            if (slider == null)
                return;
            
            sliderTooltip.Placement = PlacementMode.AbsolutePoint;
            sliderTooltip.IsOpen = true;
            UpdateTooltip(slider);

            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {

                while (Mouse.LeftButton == MouseButtonState.Pressed)
                {
                    UpdateTooltip(slider);
                    await Task.Delay(100);
                }

                sliderTooltip.IsOpen = false;
                Recalculate();

            }

        }

        void UpdateTooltip(Slider slider)
        {
            sliderTooltip.Content = slider.Value < 1.1 ? Math.Round(slider.Value * 100) + "%" : slider.Value.ToString();
            var pos = CursorUtility.GetScreenPosition();
            sliderTooltip.HorizontalOffset = pos.X + 22;
            sliderTooltip.VerticalOffset = pos.Y + 22;
        }

        private void Slider_MouseMove(object sender, MouseEventArgs e) =>
            ShowTooltip(sender as Slider);

        private void Slider_MouseLeave(object sender, MouseEventArgs e) =>
            sliderTooltip.IsOpen = false;

        #endregion
        #region Presets

        void ShowPresets()
        {
            PresetsOverlay.Show().Fade(0, 1).ConfigureAwait(false);
            presetsManager.HorizontalSlide(presetsManager.Width, 0).ConfigureAwait(false);
        }

        async void HidePresets()
        {
            _ = presetsManager.HorizontalSlide(presetsManager.Width).ConfigureAwait(false);
            (await PresetsOverlay.Fade(0)).Hide();
        }

        private void PresetsButton_Click(object sender, RoutedEventArgs e) =>
            ShowPresets();

        private void PresetsOverlay_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is Border)
                HidePresets();
        }

        #endregion

    }

}
