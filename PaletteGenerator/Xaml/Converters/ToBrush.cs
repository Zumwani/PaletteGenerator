using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace PaletteGenerator
{

    namespace Converters
    {

        class ToBrush : MarkupExtension, IValueConverter
        {

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is Color c)
                    return new SolidColorBrush(c);
                return default;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is SolidColorBrush b)
                    return b.Color;
                return default;
            }

            public override object ProvideValue(IServiceProvider serviceProvider) =>
                this;

        }

    }

}
