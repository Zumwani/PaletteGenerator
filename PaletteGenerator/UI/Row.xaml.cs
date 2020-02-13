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

        public static DependencyProperty ColumnsProperty = DependencyProperty.Register(nameof(Columns), typeof(int), typeof(Row), new PropertyMetadata(OnPropertyChanged));

        async static void OnPropertyChanged(object s, DependencyPropertyChangedEventArgs e)
            { await Task.Delay(10); (s as Row).Refresh(); }

        public int Columns
        {
            get => (int)GetValue(ColumnsProperty);
            set => SetValue(ColumnsProperty, value);
        }

        #region Colors

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

        public BindingList<Color> LeftSide { get; } = new BindingList<Color>();
        public BindingList<Color> RightSide { get; } = new BindingList<Color>();

        public Color[] AllColors =>
            LeftColor.AsArray().
            Concat(LeftSide).
            Concat(CenterColor.ApplyOffsets(ActualHueShift, 0, ActualSaturation)).
            Concat(RightSide).
            Concat(RightColor).
            ToArray();

        #endregion
        #region Hue Shift
        
        public float ActualHueShift => UseCustomHueShift ? HueShift : Global.HueShift;

        public static DependencyProperty HueShiftProperty          = DependencyProperty.Register(nameof(HueShift),          typeof(float), typeof(Row), new PropertyMetadata(0f,    OnPropertyChanged));
        public static DependencyProperty UseCustomHueShiftProperty = DependencyProperty.Register(nameof(UseCustomHueShift), typeof(bool),  typeof(Row), new PropertyMetadata(false, OnPropertyChanged));
        
        public float HueShift
        {
            get => (float)GetValue(HueShiftProperty);
            set => SetValue(HueShiftProperty, value);
        }

        public bool UseCustomHueShift
        {
            get => (bool)GetValue(UseCustomHueShiftProperty);
            set => SetValue(UseCustomHueShiftProperty, value);
        }

        #endregion
        #region Hue Offset

        public float ActualHueOffset => UseCustomHueOffset ? HueOffset : Global.HueOffset;

        public static DependencyProperty HueOffsetProperty          = DependencyProperty.Register(nameof(HueOffset),          typeof(float), typeof(Row), new PropertyMetadata(0f,    OnPropertyChanged));
        public static DependencyProperty UseCustomHueOffsetProperty = DependencyProperty.Register(nameof(UseCustomHueOffset), typeof(bool),  typeof(Row), new PropertyMetadata(false, OnPropertyChanged));

        public float HueOffset
        {
            get => (float)GetValue(HueOffsetProperty);
            set => SetValue(HueOffsetProperty, value);
        }
        
        public bool UseCustomHueOffset
        {
            get => (bool)GetValue(UseCustomHueOffsetProperty);
            set => SetValue(UseCustomHueOffsetProperty, value);
        }

        #endregion
        #region Saturation

        public float ActualSaturation => UseCustomSaturation ? Saturation : Global.Saturation;

        public static DependencyProperty SaturationProperty          = DependencyProperty.Register(nameof(Saturation),          typeof(float), typeof(Row), new PropertyMetadata(1f,    OnPropertyChanged));
        public static DependencyProperty UseCustomSaturationProperty = DependencyProperty.Register(nameof(UseCustomSaturation), typeof(bool),  typeof(Row), new PropertyMetadata(false, OnPropertyChanged));

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

        #endregion

        #endregion

        public Row()
        {

            InitializeComponent();

            LeftColorPicker.PropertyChanged     += (s, e) => Refresh();
            CenterColorPicker.PropertyChanged   += (s, e) => Refresh();
            RightColorPicker.PropertyChanged    += (s, e) => Refresh();

            Global.HueShift.PropertyChanged     += (s, e) => Refresh();
            Global.HueOffset.PropertyChanged    += (s, e) => Refresh();
            Global.Saturation.PropertyChanged   += (s, e) => Refresh();

        }

        public void Refresh()
        {

            var hueShift = ActualHueShift;
            var hueOffset = ActualHueOffset;
            var saturation = ActualSaturation;
        
            LeftColorPicker.SetOffsets(hueShift, saturation);
            CenterColorPicker.SetOffsets(hueShift, saturation);
            RightColorPicker.SetOffsets(hueShift, saturation);

            var steps = Columns / 2 + 1;
            var left =  CenterColor.Blend(LeftColor, steps).Skip(1).SkipLast(1).ToArray();
            var right = CenterColor.Blend(RightColor, steps).Skip(1).SkipLast(1).ToArray();

            left = left.ApplyOffsets(hueShift, hueOffset * 60, saturation, false);
            right = right.ApplyOffsets(hueShift, hueOffset * 60, saturation, true);

            LeftSide.Set(left.Reverse().ToArray());
            RightSide.Set(right);

        }

    }

}
