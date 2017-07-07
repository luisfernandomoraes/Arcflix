using System;
using System.Globalization;
using Xamarin.Forms;

namespace Arcflix.Converts
{
    public class DescriptionToDescritpionSuspensionPoints : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString().Length > 135)
                return value.ToString().Substring(0, 132) + "...";
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}