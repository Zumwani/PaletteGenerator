using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Palettetitizer
{

    public partial class ColorEditor : UserControl
    {

        public static DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color), typeof(Color), typeof(ColorEditor));
        public static DependencyProperty IsEditableProperty = DependencyProperty.Register(nameof(IsEditable), typeof(bool), typeof(ColorEditor));

        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public bool IsEditable
        {
            get => (bool)GetValue(IsEditableProperty);
            set => SetValue(IsEditableProperty, value);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BorderThickness = IsEditable ? new Thickness(1) : new Thickness(0);
        }

    }

}
