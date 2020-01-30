using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PaletteGenerator.UI
{

    public partial class Row
    {

        public Row() =>
            InitializeComponent();

        public void Remove(object sender, RoutedEventArgs e) =>
            Window.Remove((sender as Button)?.DataContext as Row);

        #region Properties

        public BindingList<Color> LeftSide { get; } = new BindingList<Color>();
        public BindingList<Color> RightSide { get; } = new BindingList<Color>();

        public static DependencyProperty CenterProperty = DependencyProperty.Register(nameof(Center), typeof(Color), typeof(Row), new PropertyMetadata(Colors.LightSkyBlue, OnPropertyChanged));
        public static DependencyProperty LeftProperty = DependencyProperty.Register(nameof(Left), typeof(Color), typeof(Row), new PropertyMetadata(OnPropertyChanged));
        public static DependencyProperty RightProperty = DependencyProperty.Register(nameof(Right), typeof(Color), typeof(Row), new PropertyMetadata(OnPropertyChanged));
        public static DependencyProperty HueProperty = DependencyProperty.Register(nameof(Hue), typeof(float), typeof(Row), new PropertyMetadata(OnPropertyChanged));
        public static DependencyProperty SaturationProperty = DependencyProperty.Register(nameof(Saturation), typeof(float), typeof(Row), new PropertyMetadata(OnPropertyChanged));
        public static DependencyProperty ColumnsProperty = DependencyProperty.Register(nameof(Columns), typeof(int), typeof(Row), new PropertyMetadata(OnPropertyChanged));

        static void OnPropertyChanged(object s, DependencyPropertyChangedEventArgs e)
        {
            (s as Row).Refresh();
        }

        public Color Center
        {
            get => (Color)GetValue(CenterProperty);
            set => SetValue(CenterProperty, value);
        }

        public Color Left
        {
            get => (Color)GetValue(LeftProperty);
            set => SetValue(LeftProperty, value);
        }

        public Color Right
        {
            get => (Color)GetValue(RightProperty);
            set => SetValue(RightProperty, value);
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
                l.Add(Left);
                l.AddRange(LeftSide);
                l.Add(Center);
                l.AddRange(RightSide);
                l.Add(Right);

                return l.ToArray();

            }
        }

        public void Refresh()
        {

            var steps = Columns / 2 + 1;
            LeftSide.Set(Left.Blend(Center, steps).Skip(1).SkipLast(1));
            RightSide.Set(Center.Blend(Right, steps).Skip(1).SkipLast(1));

        }

    }

}
