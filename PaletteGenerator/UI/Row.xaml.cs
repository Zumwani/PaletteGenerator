using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PaletteGenerator
{

    public partial class Row
    {

        public Row() =>
            InitializeComponent();

        private void Row_Loaded(object sender, RoutedEventArgs e) =>
            Initialize();

        private void Row_Unloaded(object sender, RoutedEventArgs e)
        {
            //LeftColor.Dispose();
            //RightColor.Dispose();
        }

        bool isInitialized;
        public void Initialize()
        {

            if (isInitialized)
                return;
            isInitialized = false;

            LeftColorPicker.PropertyChanged += (s, e) => Refresh();
            CenterColorPicker.PropertyChanged += (s, e) => Refresh();
            RightColorPicker.PropertyChanged += (s, e) => Refresh();

            Window.OnGlobalOffsetsChanged += Refresh; 

        }

        #region Properties

        public BindingList<Color> LeftSide { get; } = new BindingList<Color>();
        public BindingList<Color> RightSide { get; } = new BindingList<Color>();

        public static DependencyProperty ColumnsProperty = DependencyProperty.Register(nameof(Columns), typeof(int), typeof(Row), new PropertyMetadata(OnPropertyChanged));
        public static DependencyProperty HueProperty = DependencyProperty.Register(nameof(Hue), typeof(float), typeof(Row), new PropertyMetadata(0f, OnPropertyChanged));
        public static DependencyProperty SaturationProperty = DependencyProperty.Register(nameof(Saturation), typeof(float), typeof(Row), new PropertyMetadata(1f, OnPropertyChanged));
        public static DependencyProperty UseCustomHueProperty = DependencyProperty.Register(nameof(UseCustomHue), typeof(bool), typeof(Row), new PropertyMetadata(false, OnPropertyChanged));
        public static DependencyProperty UseCustomSaturationProperty = DependencyProperty.Register(nameof(UseCustomSaturation), typeof(bool), typeof(Row), new PropertyMetadata(false, OnPropertyChanged));
        
        async static void OnPropertyChanged(object s, DependencyPropertyChangedEventArgs e)
            { await Task.Delay(10); (s as Row).Refresh(); }

        public int Columns
        {
            get => (int)GetValue(ColumnsProperty);
            set => SetValue(ColumnsProperty, value);
        }

        public Color CenterColor
        {
            get => CenterColorPicker.Color;
            set => CenterColorPicker.Color = value;
        }

        public Color LeftColor
        {
            get => LeftColorPicker.Color;
            set => LeftColorPicker.Color = value;
        }

        public Color RightColor
        {
            get => RightColorPicker.Color;
            set => RightColorPicker.Color = value;
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

        public bool UseCustomSaturation
        {
            get => (bool)GetValue(UseCustomSaturationProperty);
            set => SetValue(UseCustomSaturationProperty, value);
        }

        public bool UseCustomHue
        {
            get => (bool)GetValue(UseCustomHueProperty);
            set => SetValue(UseCustomHueProperty, value);
        }

        #endregion

        public Color[] AllColors =>
            LeftColor.ApplyOffsets(Hue, Saturation).AsArray().
            Concat(LeftSide).
            Concat(CenterColor.ApplyOffsets(Hue, Saturation)).
            Concat(RightSide).
            Concat(RightColor.ApplyOffsets(Hue, Saturation)).
            ToArray();

        public void Refresh()
        {

            if (Columns == 0)
                return;

            var hue = UseCustomHue ? Hue : App.Window.Hue;
            var saturation = UseCustomSaturation ? Saturation : App.Window.Saturation;

            LeftColorPicker.Hue = hue;
            LeftColorPicker.Saturation = saturation;
            CenterColorPicker.Hue = hue;
            CenterColorPicker.Saturation = saturation; 
            RightColorPicker.Hue = hue;
            RightColorPicker.Saturation = saturation;

            var left = LeftColor.ApplyOffsets(hue, saturation);
            var center = CenterColor.ApplyOffsets(hue, saturation);
            var right = RightColor.ApplyOffsets(hue, saturation);

            var steps = Columns / 2 + 1;
            LeftSide.Set(left.Blend(center, steps).Skip(1).SkipLast(1));
            RightSide.Set(center.Blend(right, steps).Skip(1).SkipLast(1));

        }

        private void StackPanel_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            OffsetsButton.Visibility = Visibility.Visible;
            RemoveButton.Visibility = Visibility.Visible;
        }

        private void StackPanel_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            OffsetsButton.Visibility = Visibility.Collapsed;
            RemoveButton.Visibility = Visibility.Collapsed;
        }

    }

}
