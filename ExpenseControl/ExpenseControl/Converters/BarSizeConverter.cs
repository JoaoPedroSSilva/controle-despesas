using System.Globalization;

namespace ExpenseControl.Converters
{
    class BarSizeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2 ||
                !(values[0] is double value) ||
                !(values[1] is double maxValue) ||
                maxValue == 0)
                return 0;

            double maxWidth = 250;
            return (value / maxValue) * maxWidth;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
