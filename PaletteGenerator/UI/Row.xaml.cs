using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PaletteGenerator
{

    public partial class Row
    {

        public Row()
        {
            InitializeComponent();
            Refresh();
        }

        #region Properties

        public BindingList<Color> LeftSide { get; } = new BindingList<Color>();
        public BindingList<Color> RightSide { get; } = new BindingList<Color>();

        public static DependencyProperty ColumnsProperty = DependencyProperty.Register(nameof(Columns), typeof(int), typeof(Row), new PropertyMetadata(OnPropertyChanged));

        public static DependencyProperty CenterColorProperty = DependencyProperty.Register(nameof(CenterColor), typeof(Color), typeof(Row), new PropertyMetadata(Colors.LightSkyBlue, OnPropertyChanged));
        public static DependencyProperty LeftColorProperty = DependencyProperty.Register(nameof(LeftColor), typeof(Color), typeof(Row), new PropertyMetadata(OnPropertyChanged));
        public static DependencyProperty RightColorProperty = DependencyProperty.Register(nameof(RightColor), typeof(Color), typeof(Row), new PropertyMetadata(OnPropertyChanged));

        public static DependencyProperty GlobalHueProperty = DependencyProperty.Register(nameof(GlobalHue), typeof(float), typeof(Row), new PropertyMetadata(OnPropertyChanged));
        public static DependencyProperty GlobalSaturationProperty = DependencyProperty.Register(nameof(GlobalSaturation), typeof(float), typeof(Row), new PropertyMetadata(OnPropertyChanged));

        public static DependencyProperty CustomHueProperty = DependencyProperty.Register(nameof(CustomHue), typeof(float), typeof(Row), new PropertyMetadata(OnPropertyChanged));
        public static DependencyProperty CustomSaturationProperty = DependencyProperty.Register(nameof(CustomSaturation), typeof(float), typeof(Row), new PropertyMetadata(1f, OnPropertyChanged));

        public static DependencyProperty UseCustomHueProperty = DependencyProperty.Register(nameof(UseCustomHue), typeof(bool), typeof(Row), new PropertyMetadata(OnPropertyChanged));
        public static DependencyProperty UseCustomSaturationProperty = DependencyProperty.Register(nameof(UseCustomSaturation), typeof(bool), typeof(Row), new PropertyMetadata(OnPropertyChanged));

        async static void OnPropertyChanged(object s, DependencyPropertyChangedEventArgs e)
            { await Task.Delay(10); (s as Row).Refresh(); }

        public int Columns
        {
            get => (int)GetValue(ColumnsProperty);
            set => SetValue(ColumnsProperty, value);
        }

        public Color CenterColor
        {
            get => (Color)GetValue(CenterColorProperty);
            set => SetValue(CenterColorProperty, value);
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

        public float GlobalHue
        {
            get => (float)GetValue(GlobalHueProperty);
            set => SetValue(GlobalHueProperty, value);
        }

        public float GlobalSaturation
        {
            get => (float)GetValue(GlobalSaturationProperty);
            set => SetValue(GlobalSaturationProperty, value);
        }

        public float CustomHue
        {
            get => (float)GetValue(CustomHueProperty);
            set => SetValue(CustomHueProperty, value);
        }

        public float CustomSaturation
        {
            get => (float)GetValue(CustomSaturationProperty);
            set => SetValue(CustomSaturationProperty, value);
        }

        public bool UseCustomHue
        {
            get => (bool)GetValue(UseCustomHueProperty);
            set => SetValue(UseCustomHueProperty, value);
        }

        public bool UseCustomSaturation
        {
            get => (bool)GetValue(UseCustomSaturationProperty);
            set => SetValue(UseCustomSaturationProperty, value);
        }

        public float SelectedHue        => UseCustomHue        ? CustomHue        : GlobalHue; 
        public float SelectedSaturation => UseCustomSaturation ? CustomSaturation : GlobalSaturation;

        #endregion

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0028:Simplify collection initialization", Justification = "Less readable.")]
        public Color[] AllColors =>
            LeftColor.AsArray().
            Concat(LeftSide).
            Concat(CenterColor).
            Concat(RightSide).
            Concat(RightColor).
            ToArray();

        public void Refresh()
        {

            if (Columns == 0)
                return;

            var left = LeftColor.ApplyOffsets(SelectedHue, SelectedSaturation);
            var center = CenterColor.ApplyOffsets(SelectedHue, SelectedSaturation);
            var right = RightColor.ApplyOffsets(SelectedHue, SelectedSaturation);

            var steps = Columns / 2 + 1;
            LeftSide.Set(left.Blend(center, steps).Skip(1).SkipLast(1));
            RightSide.Set(center.Blend(right, steps).Skip(1).SkipLast(1));

            RefreshColorPicker(leftColorPicker);
            RefreshColorPicker(centerColorPicker);
            RefreshColorPicker(rightColorPicker);

        }

        void RefreshColorPicker(UI.ColorEditor colorPicker)
        {
            var rectangle = colorPicker.FindVisualChildren<Rectangle>().FirstOrDefault();
            rectangle?.GetBindingExpression(Shape.FillProperty)?.UpdateTarget();
        }

        private void StackPanel_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            offsetsButton.Visibility = Visibility.Visible;
            RemoveButton.Visibility = Visibility.Visible;
        }

        private void StackPanel_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            offsetsButton.Visibility = Visibility.Collapsed;
            RemoveButton.Visibility = Visibility.Collapsed;
        }

    }

}
