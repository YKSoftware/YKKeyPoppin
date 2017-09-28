namespace YKKeyPoppin.Views.Converters
{
    using System.Windows.Data;

    internal class BooleanToTransitDirectionConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (bool)value ? TransitionPanel.TransitDirections.ToLeft : TransitionPanel.TransitDirections.ToRight;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}
