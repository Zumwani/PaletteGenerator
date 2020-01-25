using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Linq;
using System.Windows.Media.Animation;
using System.ComponentModel;
using System;

namespace PaletteGenerator
{

    //TODO: Find theme library with sliders and replace local templates
    //TODO: Fix hue offset slider
    //TODO: Fix presets
    //TODO: Look for another color picker or create own
    //TODO: Fix installer (integrate with github and download from release branch (create dev and release branches), perhaps installer should be reuseable as well)

    public partial class MainWindow : Window
    {

        public static double MaxRows => 16;
        public static double MaxColumns => 16;
        public static double MinColumns => 2;

        public static DependencyProperty ColumnsProperty    = DependencyProperty.Register(nameof(Columns),    typeof(int),   typeof(MainWindow), new PropertyMetadata(4));      //Property change is handled by Slider.MouseUp event, since lag occurs otherwise while dragging slider
        public static DependencyProperty HueProperty        = DependencyProperty.Register(nameof(Hue),        typeof(float), typeof(MainWindow), new PropertyMetadata(1.0f));   //Property change is handled by Slider.MouseUp event, since lag occurs otherwise while dragging slider
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

        void Add(object sender = null, RoutedEventArgs e = null)
        {
            if (Rows.Count < MaxRows)
                Recalculate(Rows.Create());
        }

        public static void Remove(Row row) =>
            Current.Rows.Remove(row);

        void Update(object sender, RoutedEventArgs e) =>
            //TODO: Open installer
            MessageBox.Show("Not implemented yet. Sorry.");

        public static void Recalculate() =>
            Recalculate(Current.Rows.ToArray());

        public static async void Recalculate(params Row[] rows)
        {

            if (rows.Length == 0)
                return;

            Current.loadingOverlay.Visibility = Visibility.Visible;
            Current.loadingOverlay.BeginStoryboard((Storyboard)Current.FindResource("ShowLoadingAnimation"));

            await Task.Delay(100);

            var left = Current.LeftColor;
            var right = Current.RightColor;
            var hue = Current.Hue;

            var duration = TimeSpan.FromSeconds(0.05);
            var steps = Current.Columns / 2 + 1;
            foreach (var row in rows)
            {

                var rowTemplate = (FrameworkElement)Current.list.ItemContainerGenerator.ContainerFromItem(row);

                await Fade(rowTemplate, duration, 1, 0);
                row.SetColors(await row.Calculate(left, right, steps, hue));
                await Fade(rowTemplate, duration, 0, 1);
            
            }

            Current.loadingOverlay.BeginStoryboard((Storyboard)Current.FindResource("HideLoadingAnimation"));
            await Task.Delay(100);
            Current.loadingOverlay.Visibility = Visibility.Collapsed;

        }

        static async Task Fade(FrameworkElement element, TimeSpan duration, double from, double to)
        {

            var animation = new DoubleAnimation(from, to, duration);
            element.BeginAnimation(OpacityProperty, animation);

            await Task.Delay(duration);

        }

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

        private void Slider_MouseMove(object sender, MouseEventArgs e)
        {
            ShowTooltip(sender as Slider);
        }

        #endregion

    }

}
