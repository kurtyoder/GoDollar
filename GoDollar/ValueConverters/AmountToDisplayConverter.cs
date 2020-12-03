using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace GoDollar.ValueConverters
{
    public class AmountToDisplayConverter : BaseMultiValueConverter<AmountToDisplayConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2) throw new NotSupportedException("Must only pass Amount and IsIncome");

            decimal amount = (decimal)values[0];
            bool isIncome = (bool)values[1];

            return isIncome ? string.Format("+{0:c}", amount) : string.Format("-{0:c}", amount);
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
