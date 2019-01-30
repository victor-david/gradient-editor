/**
 * Restless Animal Development
 * This program is provided freely without restrictions, but please retain this notice in any distributions, modifications ,etc.
 * https://restlessanimal.com
 */
using System;
using System.Windows.Data;

namespace Xam.Applications.GradientEditor
{
    public class DoubleLessThan05Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is double)
            {
                return (double)value < 0.5;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
