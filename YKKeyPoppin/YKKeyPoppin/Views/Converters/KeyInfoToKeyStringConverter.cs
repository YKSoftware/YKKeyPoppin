namespace YKKeyPoppin.Views.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    internal class KeyInfoToKeyStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((KeyInfo)value).Key.ChangeString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
