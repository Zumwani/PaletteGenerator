using System.Windows;
using System.Windows.Media;
using System.Linq;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PaletteGenerator
{
    
    //TODO: Fix installer (integrate with github and download from release branch (create dev and release branches), perhaps installer should be reuseable as well)

    public partial class Window : System.Windows.Window
    {

        #region Properties

        public double MaxRows => 16;
        public double MaxColumns => 16;
        public double MinColumns => 2;

        public static DependencyProperty ColumnsProperty    = DependencyProperty.Register(nameof(Columns),    typeof(int),   typeof(Window), new PropertyMetadata(4, OnColumnsChanged));      
        public static DependencyProperty HueProperty        = DependencyProperty.Register(nameof(Hue),        typeof(float), typeof(Window), new PropertyMetadata(0f, OnColumnsChanged));   
        public static DependencyProperty SaturationProperty = DependencyProperty.Register(nameof(Saturation), typeof(float), typeof(Window), new PropertyMetadata(1f, OnColumnsChanged));   
        public static DependencyProperty LeftColorProperty  = DependencyProperty.Register(nameof(LeftColor),  typeof(Color), typeof(Window), new PropertyMetadata(Colors.White, OnColumnsChanged));
        public static DependencyProperty RightColorProperty = DependencyProperty.Register(nameof(RightColor), typeof(Color), typeof(Window), new PropertyMetadata(Colors.Black, OnColumnsChanged));

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

        void Add(object sender = null, RoutedEventArgs e = null)
        {
            if (Rows.Count < MaxRows)
                using (LoadingUtility.KeepLoadingScreenHidden) 
                { Recalculate(Rows.Create()); };
        }

        public static void Remove(Row row) =>
            App.Window.Rows.Remove(row);

        public static void Recalculate() =>
            Recalculate(App.Window.Rows.ToArray());

        static int recalculationCount;
        public static void Recalculate(params Row[] rows)
        {

            if (rows.Length == 0)
                return;

            recalculationCount += 1;
            var currentCount = recalculationCount;

            var left = App.Window.LeftColor;
            var right = App.Window.RightColor;

            var tasks = rows.Select(async Task => 
            {

                var steps = App.Window.Columns / 2 + 1;
                foreach (var row in rows)
                {

                    if (recalculationCount != currentCount)
                        return System.Threading.Tasks.Task.CompletedTask;

                    var rowTemplate = App.Dispatcher.Invoke(() => 
                        (FrameworkElement)App.Window.list.ItemContainerGenerator.ContainerFromItem(row));

                    row.SetColors(await row.Calculate(left, right, steps));

                }

                return System.Threading.Tasks.Task.CompletedTask;

            });

            Task.WhenAll(tasks).ContinueWith(t => UI.ColorEditor.RefreshAll()).ConfigureAwait(false);

        }

        #endregion
        #region Window

        public Window() =>
            InitializeComponent();

        void Update(object sender, RoutedEventArgs e) =>
            Process.Start("explorer.exe", "https://github.com/Zumwani/PaletteGenerator");

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Add(); Add();
        }

        #endregion

    }

}
