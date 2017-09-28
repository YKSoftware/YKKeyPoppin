namespace YKKeyPoppin.Views.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Input;

    internal class ModifierKeyInKeyInfoToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (parameter as string)
            {
                case "C": return ((KeyInfo)value).ModifierKeys.HasFlag(ModifierKeys.Control) ? Visibility.Visible : Visibility.Collapsed;
                case "S": return ((KeyInfo)value).ModifierKeys.HasFlag(ModifierKeys.Shift) ? Visibility.Visible : Visibility.Collapsed;
                case "A": return ((KeyInfo)value).ModifierKeys.HasFlag(ModifierKeys.Alt) ? Visibility.Visible : Visibility.Collapsed;
                case "W": return ((KeyInfo)value).ModifierKeys.HasFlag(ModifierKeys.Windows) ? Visibility.Visible : Visibility.Collapsed;
                default: return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
