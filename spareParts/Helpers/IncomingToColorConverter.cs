using System;

namespace spareParts.Helpers;

public class IncomingToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        bool IsIncoming = (bool)value;
        // Incoming messages = Grey/Light, Outgoing = Blue/Green
        return IsIncoming ? Color.FromArgb("#E9E9EB") : Color.FromArgb("#2196F3");
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
