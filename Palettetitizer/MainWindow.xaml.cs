using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Palettetitizer
{

    public partial class MainWindow : Window
    {

        public static DependencyProperty ColumnsProperty    = DependencyProperty.Register(nameof(Columns), typeof(int), typeof(MainWindow), new PropertyMetadata(6, OnColumnsChanged));
        public static DependencyProperty LeftColorProperty  = DependencyProperty.Register(nameof(LeftColor), typeof(Color), typeof(MainWindow), new PropertyMetadata(Colors.White, OnColumnsChanged));
        public static DependencyProperty RightColorProperty = DependencyProperty.Register(nameof(RightColor), typeof(Color), typeof(MainWindow), new PropertyMetadata(Colors.Black, OnColumnsChanged));

        static void OnColumnsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var window = (MainWindow)sender;
            foreach (var row in window.Rows)
                row.Recalculate(window.Columns, window.LeftColor, window.RightColor);
        }

        public int Columns
        { get => (int)GetValue(ColumnsProperty); set => SetValue(ColumnsProperty, value); }

        public ObservableCollection<Row> Rows { get; } = new ObservableCollection<Row>();
        
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

        void Add(object sender = null, RoutedEventArgs e = null) =>
            Rows.Add(new Row(Columns, LeftColor, RightColor));

        public static void Remove(Row row) =>
            Current.Rows.Remove(row);

        void Update(object sender, RoutedEventArgs e)
        {
            //TODO: Open installer
            MessageBox.Show("Not implemented yet. Sorry.");
        }

        #region Window

        public static MainWindow Current { get; private set; }

        public MainWindow()
        {
            Current = this;
            Unloaded += (s,e) => { if (Current == this) Current = null; };
            Rows = new ObservableCollection<Row>();
            Add(); Add(); Add();
        }

        void Minimize(object sender, RoutedEventArgs e) =>
            WindowState = WindowState.Minimized;

        void Close(object sender, RoutedEventArgs e) =>
            Close();

        private void DockPanel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource == sender || e.Source is TextBlock)
                DragMove();
        }

        #endregion

    }

}
