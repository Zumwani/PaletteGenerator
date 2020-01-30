using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

        public static DependencyProperty CenterColorProperty = DependencyProperty.Register(nameof(CenterColor), typeof(Color), typeof(Row), new PropertyMetadata(Colors.LightSkyBlue, OnPropertyChanged));
        public static DependencyProperty LeftColorProperty = DependencyProperty.Register(nameof(LeftColor), typeof(Color), typeof(Row), new PropertyMetadata(OnPropertyChanged));
        public static DependencyProperty RightColorProperty = DependencyProperty.Register(nameof(RightColor), typeof(Color), typeof(Row), new PropertyMetadata(OnPropertyChanged));
        public static DependencyProperty HueProperty = DependencyProperty.Register(nameof(Hue), typeof(float), typeof(Row), new PropertyMetadata(OnPropertyChanged));
        public static DependencyProperty SaturationProperty = DependencyProperty.Register(nameof(Saturation), typeof(float), typeof(Row), new PropertyMetadata(OnPropertyChanged));
        public static DependencyProperty ColumnsProperty = DependencyProperty.Register(nameof(Columns), typeof(int), typeof(Row), new PropertyMetadata(OnPropertyChanged));

        static void OnPropertyChanged(object s, DependencyPropertyChangedEventArgs e)
        {
            (s as Row).Refresh();
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

        public int Columns
        {
            get => (int)GetValue(ColumnsProperty);
            set => SetValue(ColumnsProperty, value);
        }

        #endregion

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0028:Simplify collection initialization", Justification = "Less readable.")]
        public Color[] AllColors
        {
            get
            {

                var l = new List<Color>();
                l.Add(LeftColor);
                l.AddRange(LeftSide);
                l.Add(CenterColor);
                l.AddRange(RightSide);
                l.Add(RightColor);

                return l.ToArray();

            }
        }

        public void Refresh()
        {

            if (Columns == 0)
                return;

            var steps = Columns / 2 + 1;
            LeftSide.Set(LeftColor.Blend(CenterColor, steps).Skip(1).SkipLast(1));
            RightSide.Set(CenterColor.Blend(RightColor, steps).Skip(1).SkipLast(1));

            RefreshColorPicker(leftColorPicker);
            RefreshColorPicker(centerColorPicker);
            RefreshColorPicker(rightColorPicker);

        }

        void RefreshColorPicker(UI.ColorEditor colorPicker)
        {
            var rectangle = colorPicker.FindVisualChildren<Rectangle>().FirstOrDefault();
            rectangle?.GetBindingExpression(Shape.FillProperty)?.UpdateTarget();
        }

    }

}
