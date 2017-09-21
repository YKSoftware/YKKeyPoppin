namespace YKKeyPoppin.Views.Converters
{
    using System.Windows.Data;
    using System.Windows.Media;

    internal class ComboToBrushConverter : IValueConverter
    {
        private static SolidColorBrush defaultForeground = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0x7f, 0x50));
        private static SolidColorBrush excellentForeground = new SolidColorBrush(Color.FromArgb(0xff, 0x26, 0x83, 0xc6));
        private static SolidColorBrush superForeground = new SolidColorBrush(Color.FromArgb(0xff, 0xe3, 0x80, 0x25));
        private static SolidColorBrush amazingForeground = new SolidColorBrush(Color.FromArgb(0xff, 0x5b, 0xe3, 0x2d));
        private static SolidColorBrush godForeground = new SolidColorBrush(Color.FromArgb(0xff, 0xdb, 0xe3, 0x2d));

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var comboLevel = (int)value;
            switch (comboLevel)
            {
                case 0: return defaultForeground;
                case 1: return defaultForeground;
                case 2: return excellentForeground;
                case 3: return superForeground;
                case 4: return amazingForeground;
                default: return godForeground;
            }
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}
