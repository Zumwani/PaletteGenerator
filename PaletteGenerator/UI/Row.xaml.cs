using PaletteGenerator.Utilities;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace PaletteGenerator.Models
{

    internal partial class Row
    {

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

        public float ActualHue => UseCustomHue ? Hue : Global.Hue;
        public float ActualSaturation => UseCustomSaturation ? Saturation : Global.Saturation;

        public Color[] AllColors =>
            LeftColor.ApplyOffsets(ActualHue, ActualSaturation).AsArray().
            Concat(LeftSide).
            Concat(CenterColor.ApplyOffsets(ActualHue, ActualSaturation)).
            Concat(RightSide).
            Concat(RightColor.ApplyOffsets(ActualHue, ActualSaturation)).
            ToArray();

        #endregion

        public Row()
        {

            InitializeComponent();

            LeftColorPicker.PropertyChanged += (s, e) => Refresh();
            CenterColorPicker.PropertyChanged += (s, e) => Refresh();
            RightColorPicker.PropertyChanged += (s, e) => Refresh();

            Global.Hue.PropertyChanged += (s, e) => Refresh();
            Global.Saturation.PropertyChanged += (s, e) => Refresh();

        }

        public void Refresh()
        {

            if (Columns == 0)
                return;

            var hue = ActualHue;
            var saturation = ActualSaturation;

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

    }

}
