using System.Globalization;

namespace ExpenseControl.Converters
{
    class ValueToWidthConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is double doubleValue)
            {
                return doubleValue * 10;
            }
            double amount = (double)value;
            return amount * 2;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
