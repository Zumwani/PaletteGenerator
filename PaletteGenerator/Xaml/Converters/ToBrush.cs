using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace PaletteGenerator
{

    namespace Converters
    {

        public class ToBrush : MarkupExtension, IValueConverter
        {

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is Color c)
                    return new SolidColorBrush(c.OffsetHue(WindowUtility.Current.Hue).OffsetSaturation(WindowUtility.Current.Saturation));
                return default;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }

            public override object ProvideValue(IServiceProvider serviceProvider) =>
                this;

        }

    }

}
