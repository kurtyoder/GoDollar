using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;

namespace GoDollar.ValueConverters
{
    public class BoolToCollapsedConverter : BaseValueConverter<BoolToCollapsedConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
