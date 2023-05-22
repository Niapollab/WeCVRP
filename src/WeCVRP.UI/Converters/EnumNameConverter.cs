using System.Globalization;

namespace WeCVRP.UI.Converters;

public class EnumNameConverter<T> : ValueConverter<T, string>
{
    public override string Convert(T value, object parameter, CultureInfo culture)
        => Enum.GetName(typeof(T), value!) ?? throw new ArgumentException($"Unexpected enum value \"{value}\".");

    public override T ConvertBack(string value, object parameter, CultureInfo culture)
        => (T)Enum.Parse(typeof(T), value);
}
