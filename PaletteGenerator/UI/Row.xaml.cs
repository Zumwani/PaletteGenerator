using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PaletteGenerator
{

    public class CustomValue<T> : INotifyPropertyChanged, IDisposable
    {
        
        /// <summary>Binds to global value, one way.</summary>
        public static CustomValue<T> FromGlobal(DependencyObject source, DependencyProperty property, bool setUseCustomAutomatically = true)
        {

            var v = new CustomValue<T> 
            { Global = (T)property.DefaultMetadata.DefaultValue };
            v.custom = v.Global;

            v.descriptor = DependencyPropertyDescriptor.FromProperty(property, source.GetType());
            v.descriptor.AddValueChanged(source, v.OnSourceChanged);

            v.source = source;
            v.property = property;
            v.setUseCustomAutomatically = setUseCustomAutomatically;

            return v;

        }

        /// <summary>Binds to global value, two way.</summary>
        public static CustomValue<T> FromToGlobal(DependencyObject source, DependencyProperty property, bool setUseCustomAutomatically = true)
        {

            var v = FromGlobal(source, property, setUseCustomAutomatically);
            v.isTwoWay = !v.descriptor.IsReadOnly;
            v.CanUseCustom = false;
            return v;

        }

        void OnSourceChanged(object s, EventArgs e)
        {
            Global = (T)source.GetValue(property);
            //custom = global;
        }

        public CustomValue() { }
        public CustomValue(T custom) => Custom = custom;

        DependencyObject source;
        DependencyProperty property;
        DependencyPropertyDescriptor descriptor;
        bool isTwoWay;
        bool setUseCustomAutomatically;

        T global;
        T custom;
        bool useCustom;

        [JsonIgnore]
        public Action OnValueChanged { get; set; }

        [JsonIgnore]
        public T Global
        {
            get => global;
            set { global = value; OnPropertyChanged(); }
        }

        public T Custom
        {
            get => custom;
            set { custom = value; OnPropertyChanged(); }
        }

        public bool UseCustom
        {
            get => useCustom;
            set { useCustom = value; OnPropertyChanged(); }
        }

        public bool IsCustom => UseCustom && CanUseCustom;
        public bool IsGlobal => !UseCustom;
        public bool CanUseCustom { get; set; } = true;

        [JsonIgnore]
        public T Selected => IsCustom ? Custom : Global;

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string name = "")
        {

            if (name == nameof(Custom) && setUseCustomAutomatically)
                if (CanUseCustom)
                    useCustom = true;
                else
                    Global = Custom;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsCustom)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsGlobal)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Selected)));

            OnValueChanged?.Invoke();

            if (isTwoWay)
                source?.SetValue(property, Selected);

        }

        public void Reset() =>
            UseCustom = false;

        public static implicit operator T(CustomValue<T> value) =>
            value.Selected;

        public static implicit operator CustomValue<T>(T value) =>
            new CustomValue<T>(value);

        public void Set(CustomValue<T> custom) =>
            Set(custom.Custom, custom.UseCustom);

        public void Set(T custom, bool useCustom)
        {
            Custom = custom;
            UseCustom = useCustom;
        }

        public void Dispose() =>
            descriptor.RemoveValueChanged(source, OnSourceChanged);

    }

    public partial class Row
    {

        private void Row_Loaded(object sender, RoutedEventArgs e)
        {
            CenterColorPicker.Color = CustomValue<Color>.FromToGlobal(this, CenterColorProperty);
            LeftColor.OnValueChanged    = Refresh;
            RightColor.OnValueChanged   = Refresh;
            Hue.OnValueChanged          = Refresh;
            Saturation.OnValueChanged   = Refresh;
            Refresh();
        }

        private void Row_Unloaded(object sender, RoutedEventArgs e)
        {
            LeftColor.Dispose();
            RightColor.Dispose();
        }

        public Row() =>
            InitializeComponent();

        #region Properties

        public BindingList<Color> LeftSide { get; } = new BindingList<Color>();
        public BindingList<Color> RightSide { get; } = new BindingList<Color>();

        public static DependencyProperty ColumnsProperty = DependencyProperty.Register(nameof(Columns), typeof(int), typeof(Row), new PropertyMetadata(OnPropertyChanged));
        public static DependencyProperty CenterColorProperty = DependencyProperty.Register(nameof(CenterColor), typeof(Color), typeof(Row), new PropertyMetadata(Colors.LightSkyBlue, OnPropertyChanged));
        
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

        public CustomValue<Color> LeftColor  { get; } = CustomValue<Color>.FromGlobal(App.Window, Window.LeftColorProperty);
        public CustomValue<Color> RightColor { get; } = CustomValue<Color>.FromGlobal(App.Window, Window.RightColorProperty);

        public CustomValue<float> Hue        { get; } = CustomValue<float>.FromGlobal(App.Window, Window.HueProperty, false);
        public CustomValue<float> Saturation { get; } = CustomValue<float>.FromGlobal(App.Window, Window.SaturationProperty, false);

        #endregion

        public Color[] AllColors =>
            LeftColor.Selected.ApplyOffsets(Hue, Saturation).AsArray().
            Concat(LeftSide).
            Concat(CenterColor.ApplyOffsets(Hue, Saturation)).
            Concat(RightSide).
            Concat(RightColor.Selected.ApplyOffsets(Hue, Saturation)).
            ToArray();

        public void Refresh()
        {

            if (Columns == 0)
                return;

            var left = LeftColor.Selected.ApplyOffsets(Hue, Saturation);
            var center = CenterColor.ApplyOffsets(Hue, Saturation);
            var right = RightColor.Selected.ApplyOffsets(Hue, Saturation);

            var steps = Columns / 2 + 1;
            LeftSide.Set(left.Blend(center, steps).Skip(1).SkipLast(1));
            RightSide.Set(center.Blend(right, steps).Skip(1).SkipLast(1));

            RefreshColorPicker(LeftColorPicker, left);
            RefreshColorPicker(CenterColorPicker, center);
            RefreshColorPicker(RightColorPicker, right);

        }

        void RefreshColorPicker(UI.ColorEditor colorPicker, Color color)
        {
            var rectangle = colorPicker.FindVisualChildren<Rectangle>().FirstOrDefault();
            rectangle?.SetValue(Shape.FillProperty, new SolidColorBrush(color));
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
