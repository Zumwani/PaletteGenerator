using System.Windows;
using System.Windows.Media;
using System.ComponentModel;

namespace PaletteGenerator
{
    
    //TODO: Fix installer (integrate with github and download from release branch (create dev and release branches), perhaps installer should be reuseable as well)
    //TODO: Change all x:Name to PascalCase
    //TODO: Fix attached property for popups
    //TODO: Allow for choosing cell size when exporting

    public partial class Window : System.Windows.Window
    {

        #region Properties

        public int MaxRows => 16;
        public int MaxColumns => 16;
        public int MinColumns => 2;
        public string Github => "https://github.com/Zumwani/PaletteGenerator";
        public string Discord => "https://discord.gg/P8VX7Wb";
        public string Version => typeof(Window).Assembly.GetName().Version.ToString();

        public static DependencyProperty ColumnsProperty    = DependencyProperty.Register(nameof(Columns),    typeof(int),   typeof(Window), new PropertyMetadata(4));      
        public static DependencyProperty HueProperty        = DependencyProperty.Register(nameof(Hue),        typeof(float), typeof(Window), new PropertyMetadata(0f));   
        public static DependencyProperty SaturationProperty = DependencyProperty.Register(nameof(Saturation), typeof(float), typeof(Window), new PropertyMetadata(1f));   
        public static DependencyProperty LeftColorProperty  = DependencyProperty.Register(nameof(LeftColor),  typeof(Color), typeof(Window), new PropertyMetadata(Colors.White));
        public static DependencyProperty RightColorProperty = DependencyProperty.Register(nameof(RightColor), typeof(Color), typeof(Window), new PropertyMetadata(Colors.Black));

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
        #region Window

        public Window() =>
            InitializeComponent();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LeftColorPicker.Color  = CustomValue<Color>.FromToGlobal(this, LeftColorProperty);
            RightColorPicker.Color = CustomValue<Color>.FromToGlobal(this, RightColorProperty);
            Commands.AddRow.Execute();
            Commands.AddRow.Execute();
        }

        #endregion

    }

}
