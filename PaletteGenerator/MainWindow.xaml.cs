using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace PaletteGenerator
{

    //TODO: Fix hue offset slider
    //TODO: Fix presets
    //TODO: Look for another color picker or create own
    //TODO: Fix installer (integrate with github and download from release branch (create dev and release branches), perhaps installer should be reuseable as well)

    public partial class MainWindow : Window
    {

        public static double MaxRows => 16;
        public static double MaxColumns => 16;
        public static double MinColumns => 2;

        public static DependencyProperty ColumnsProperty    = DependencyProperty.Register(nameof(Columns),    typeof(int),   typeof(MainWindow), new PropertyMetadata(8));      //Property change is handled by Slider.MouseUp event, since lag occurs otherwise while dragging slider
        public static DependencyProperty HueProperty        = DependencyProperty.Register(nameof(Hue),        typeof(float), typeof(MainWindow), new PropertyMetadata(1.0f));   //Property change is handled by Slider.MouseUp event, since lag occurs otherwise while dragging slider
        public static DependencyProperty LeftColorProperty  = DependencyProperty.Register(nameof(LeftColor),  typeof(Color), typeof(MainWindow), new PropertyMetadata(Colors.White, OnColumnsChanged));
        public static DependencyProperty RightColorProperty = DependencyProperty.Register(nameof(RightColor), typeof(Color), typeof(MainWindow), new PropertyMetadata(Colors.Black, OnColumnsChanged));

        static void OnColumnsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) =>
            Recalculate();

        public ObservableCollection<Row> Rows { get; } = new ObservableCollection<Row>();

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
                Rows.Add(new Row(Columns, LeftColor, RightColor, Hue));
        }

        public static void Remove(Row row) =>
            Current.Rows.Remove(row);

        void Update(object sender, RoutedEventArgs e)
        {
            //TODO: Open installer
            MessageBox.Show("Not implemented yet. Sorry.");
        }

        public static void Recalculate() =>
            Current.Rows.ForEach(Recalculate);

        public static void Recalculate(Row row) =>
            row.Recalculate(Current.Columns, Current.LeftColor, Current.RightColor, Current.Hue).ConfigureAwait(false);

        #region Window

        public static MainWindow Current { get; private set; }

        public MainWindow()
        {
            Current = this;
            Unloaded += (s,e) => { if (Current == this) Current = null; };
            Rows = new ObservableCollection<Row>();
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

        private void Slider_MouseUp(object sender, MouseButtonEventArgs e) =>
            Recalculate(); 
        
        private void Slider_KeyUp(object sender, KeyEventArgs e) =>
            Recalculate();

        readonly ToolTip sliderTooltip = new ToolTip();
        private void Slider_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) =>
            ShowTooltip(sender as Slider);

        void ShowTooltip(Slider slider)
        {

            if (slider == null)
                return;

            var pos = CursorUtility.GetScreenPosition();
            sliderTooltip.Placement = PlacementMode.AbsolutePoint;
            sliderTooltip.HorizontalOffset = pos.X + 22;
            sliderTooltip.VerticalOffset = pos.Y + 22;

            sliderTooltip.Content = slider.Value;
            sliderTooltip.IsOpen = true;

        }

        private void Slider_MouseLeave(object sender, MouseEventArgs e) =>
            sliderTooltip.IsOpen = false;

        #endregion
    }

}
