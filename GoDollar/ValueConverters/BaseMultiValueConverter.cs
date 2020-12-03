using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;

namespace GoDollar.ValueConverters
{
    public abstract class BaseMultiValueConverter<T> : MarkupExtension, IMultiValueConverter where T : class, new()
    {
        public static T converter = null;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return converter ??= new T();
        }

        public abstract object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);   

        public abstract object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture);
       
    }
}
