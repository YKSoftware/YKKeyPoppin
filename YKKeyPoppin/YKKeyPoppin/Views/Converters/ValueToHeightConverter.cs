namespace YKKeyPoppin.Views.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    internal class ValueToHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = (double)value;
            var max = double.Parse(parameter as string);
            var height = v * max / 100;
            if ((v > 0) && (height < 1)) height = 1;
            return height;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
