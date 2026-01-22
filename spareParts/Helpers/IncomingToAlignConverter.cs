using System;

namespace spareParts.Helpers;

public class IncomingToAlignConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        bool IsIncoming = (bool)value;
        // If it's incoming (from the other person), put it on the Left (Start)
        // If it's outgoing (from you), put it on the Right (End)
        return IsIncoming ? LayoutOptions.Start : LayoutOptions.End;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
