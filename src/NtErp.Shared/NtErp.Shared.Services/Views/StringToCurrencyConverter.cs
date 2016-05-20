using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace NtErp.Shared.Services.Views {
    public class StringToCurrencyConverter : IValueConverter {
        private bool _addedZero = false;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            decimal val = (decimal)value;

            string result = val.ToString();

            if (_addedZero) {
                result = CutOffLastChar(result);
                _addedZero = false;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            string rawValue = SanitizeValue(value);

            decimal dec = 0;
            Decimal.TryParse(SanitizeValue(rawValue), out dec);

            return dec;
        }

        private string SanitizeValue(object decimalValue) {
            string val = (string)decimalValue;

            // If the string ends with a decimal separator, the default converter behaviour is to skip
            // that last char. Therefore we will add a '0' automatically so that the resulting value will be 
            // a valid decimal value. 
            // But we do not want to display that '0' in the GUI TextBox until an actual '0' or another 
            // number has been typed in by the user. 
            // When adding that '0' we set the _addedZero flag so that the Convert method, when running the next 
            // time will cut off the automatically added '0'.
            if (val.EndsWith(NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator)) {
                // If the input value ends with a decimal separator AND contains more than one decimal separator
                if (val.Count(c => c.ToString() == NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator) > 1) {
                    val = CutOffLastChar(val);
                } else {
                    val += "0";
                    _addedZero = true;
                }
            } else if (val.Contains(NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator) && CheckDecimalPlacesCount(val) > 2) {
                val = TrimToTwoDecimalPlaces(val);
            }

            return val;
        }

        private int CheckDecimalPlacesCount(string value) {
            return value.Length - 1 - value.IndexOf(NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator);
        }

        private string CutOffLastChar(string value) {
            return value.Substring(0, value.Length - 1);
        }

        private string TrimToTwoDecimalPlaces(string value) {
            return value.Substring(0, value.IndexOf(",") + 3);
        }
    }
}
