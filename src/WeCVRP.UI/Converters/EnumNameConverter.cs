using System.Globalization;

namespace WeCVRP.UI.Converters;

public class EnumNameConverter<T> : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType != typeof(string))
            throw new ArgumentException($"\"{nameof(targetType)}\" must be \"{typeof(string).Name}\" type.", nameof(targetType));

        if (value is not T typedValue)
            throw new ArgumentException($"\"{nameof(value)}\" must has \"{typeof(T).Name}\" type.", nameof(value));

        return Convert(typedValue, parameter, culture);
    }

    public virtual string Convert(T value, object parameter, CultureInfo culture)
        => Enum.GetName(typeof(T), value!) ?? throw new ArgumentException($"Unexpected enum value \"{value}\".");

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType != typeof(T))
            throw new ArgumentException($"\"{nameof(targetType)}\" must be \"{typeof(T).Name}\" type.", nameof(targetType));

        if (value is not string typedValue)
            throw new ArgumentException($"\"{nameof(value)}\" must has \"{typeof(string).Name}\" type.", nameof(value));

        return ConvertBack(typedValue, parameter, culture)!;
    }

    public virtual T ConvertBack(string value, object parameter, CultureInfo culture)
        => (T)Enum.Parse(typeof(T), value);
}
