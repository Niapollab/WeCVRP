using System.Globalization;

namespace WeCVRP.UI.Converters;

public abstract class ValueConverter<TFrom, TTo> : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType != typeof(TTo))
            throw new ArgumentException($"\"{nameof(targetType)}\" must be \"{typeof(TTo).Name}\" type.", nameof(targetType));

        if (value is not TFrom typedValue)
            throw new ArgumentException($"\"{nameof(value)}\" must has \"{typeof(TFrom).Name}\" type.", nameof(value));

        return Convert(typedValue, parameter, culture);
    }

    public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType != typeof(TFrom))
            throw new ArgumentException($"\"{nameof(targetType)}\" must be \"{typeof(TFrom).Name}\" type.", nameof(targetType));

        if (value is not TTo typedValue)
            throw new ArgumentException($"\"{nameof(value)}\" must has \"{typeof(TTo).Name}\" type.", nameof(value));

        return ConvertBack(typedValue, parameter, culture);
    }

    public abstract TTo Convert(TFrom value, object parameter, CultureInfo culture);

    public abstract TFrom ConvertBack(TTo value, object parameter, CultureInfo culture);
}
