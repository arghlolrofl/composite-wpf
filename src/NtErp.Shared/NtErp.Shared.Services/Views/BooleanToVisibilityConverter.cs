using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NtErp.Shared.Services.Views {
    public class BooleanToVisibilityConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            bool val = (bool)value;

            Visibility vis = val ? Visibility.Visible : Visibility.Collapsed;

            return vis;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
