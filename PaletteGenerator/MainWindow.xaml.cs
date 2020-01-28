using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Linq;
using System.ComponentModel;
using System.Diagnostics;

namespace PaletteGenerator
{

    //TODO: Look for another color picker or create own
    //TODO: Fix installer (integrate with github and download from release branch (create dev and release branches), perhaps installer should be reuseable as well)
    //TODO: Fix color buttons when hovering at edge

    public partial class MainWindow : Window
    {

        #region Properties

        public double MaxRows => 16;
        public double MaxColumns => 16;
        public double MinColumns => 2;

        public static DependencyProperty ColumnsProperty    = DependencyProperty.Register(nameof(Columns),    typeof(int),   typeof(MainWindow), new PropertyMetadata(4));      //Property change is handled by Slider.MouseUp event, since lag occurs otherwise while dragging slider
        public static DependencyProperty HueProperty        = DependencyProperty.Register(nameof(Hue),        typeof(float), typeof(MainWindow), new PropertyMetadata(0f));   //Property change is handled by Slider.MouseUp event, since lag occurs otherwise while dragging slider
        public static DependencyProperty SaturationProperty = DependencyProperty.Register(nameof(Saturation), typeof(float), typeof(MainWindow), new PropertyMetadata(1f));   //Property change is handled by Slider.MouseUp event, since lag occurs otherwise while dragging slider
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

        public float Saturation
        {
            get => (float)GetValue(SaturationProperty);
            set => SetValue(SaturationProperty, value);
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

        public static Row[] CurrentRows
        {
            get => WindowUtility.Current.Rows.ToArray();
            set => WindowUtility.Current.Rows.Set(value);
        }

        void Add(object sender = null, RoutedEventArgs e = null)
        {
            if (Rows.Count < MaxRows)
                using (LoadingUtility.KeepLoadingScreenHidden) 
                { Recalculate(Rows.Create()); };
        }

        public static void Remove(Row row) =>
            WindowUtility.Current.Rows.Remove(row);

        public static void Recalculate() =>
            Recalculate(WindowUtility.Current.Rows.ToArray());

        static int recalculationCount;
        public static void Recalculate(params Row[] rows)
        {

            if (rows.Length == 0)
                return;

            recalculationCount += 1;
            var currentCount = recalculationCount;

            var left =  WindowUtility.Current.LeftColor;
            var right = WindowUtility.Current.RightColor;
            var hue =   WindowUtility.Current.Hue;
            var sat =   WindowUtility.Current.Saturation;

            var tasks = rows.Select(async Task => 
            {

                var steps = WindowUtility.Current.Columns / 2 + 1;
                foreach (var row in rows)
                {

                    if (recalculationCount != currentCount)
                        return System.Threading.Tasks.Task.CompletedTask;

                    var rowTemplate = App.Current.Dispatcher.Invoke(() => 
                        (FrameworkElement)WindowUtility.Current.list.ItemContainerGenerator.ContainerFromItem(row));

                    row.SetColors(await row.Calculate(left, right, steps, hue, sat));

                }

                return System.Threading.Tasks.Task.CompletedTask;

            });

            tasks.ForEach(t => t.ConfigureAwait(false));

        }

        #endregion
        #region Window

        public MainWindow()
        {
            InitializeComponent();
            Add(); Add();
        }

        void Update(object sender, RoutedEventArgs e) =>
            Process.Start("explorer.exe", "https://github.com/Zumwani/PaletteGenerator");

        private void Slider_DragCompleted(object sender, DragCompletedEventArgs e) =>
            Recalculate();

        private void Slider_KeyUp(object sender, KeyEventArgs e) =>
            Recalculate();

        #endregion

    }

}
