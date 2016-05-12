using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace NtErp.Shared.Services.Views {
    public class MonetaryValueToForegroundColorConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            decimal cashBalance = (decimal)value;

            Color color = cashBalance >= 0 ? Colors.Green : Colors.Red;

            return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
