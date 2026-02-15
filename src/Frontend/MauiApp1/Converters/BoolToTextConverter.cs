using System.Globalization;

namespace MauiApp1.Converters;

public class BoolToTextConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not bool boolValue || parameter is not string paramString) return string.Empty;

        var options = paramString.Split('|');
        return options.Length == 2 ? (boolValue ? options[0] : options[1]) : string.Empty;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
