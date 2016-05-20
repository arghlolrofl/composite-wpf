using NtErp.Shared.Services.Views;
using NUnit.Framework;
using System.Globalization;

namespace NtErp.Shared.Services.Tests {

    [TestFixture]
    public class StringToCurrencyConverterFixture {
        static string sep = NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator;
        static decimal decimalTestValue = 123.45m;
        static string stringTestValue = "123" + sep + "45";

        [Test]
        public void ConvertsForthDecimalValueToString() {
            StringToCurrencyConverter converter = new StringToCurrencyConverter();

            string result = converter.Convert(decimalTestValue, null, null, CultureInfo.CurrentCulture) as string;

            Assert.IsNotNull(result, "Input value could not be casted to string.");
            Assert.AreEqual(stringTestValue, result, "Invalid forth conversion from decimal to string.");
        }

        [Test]
        public void ConvertsBackStringToDecimalValue() {
            StringToCurrencyConverter converter = new StringToCurrencyConverter();

            decimal result = (decimal)converter.ConvertBack(stringTestValue, null, null, CultureInfo.CurrentCulture);

            Assert.AreEqual(decimalTestValue, result, "Invalid back conversion from string to decimal.");
        }

        [Test]
        public void ConvertsBackAndTrimsRedundantDecimalPlaces() {
            StringToCurrencyConverter converter = new StringToCurrencyConverter();

            string testString = "123" + sep + "4567";

            decimal result = (decimal)converter.ConvertBack(testString, null, null, CultureInfo.CurrentCulture);

            Assert.AreEqual(123.45m, result);
        }

        [Test]
        public void ConvertsBackAndIgnoresTrailingDecimalSeparator() {
            StringToCurrencyConverter converter = new StringToCurrencyConverter();

            string testString = "123" + sep;

            decimal result = (decimal)converter.ConvertBack(testString, null, null, CultureInfo.CurrentCulture);

            Assert.AreEqual(123.0m, result);
        }

        [Test]
        public void ConvertsBackWithTrailingDecimalSeparatorAndAddsZero() {
            StringToCurrencyConverter converter = new StringToCurrencyConverter();

            string testString = "123" + sep;

            decimal result = (decimal)converter.ConvertBack(testString, null, null, CultureInfo.CurrentCulture);
            string strResult = result.ToString();

            Assert.AreEqual("123" + sep + "0", strResult);
        }
    }
}