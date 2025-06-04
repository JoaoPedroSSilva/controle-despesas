using System;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Maui.Controls;

namespace ExpenseControl.Converters
{
    class StringNullOrEmptyToBoolConverter : IValueConverter
    {
        public bool Invert { get; set; } = false;
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            bool isNullOrEmpty = string.IsNullOrEmpty(value?.ToString());
            return Invert ? !isNullOrEmpty : isNullOrEmpty;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
