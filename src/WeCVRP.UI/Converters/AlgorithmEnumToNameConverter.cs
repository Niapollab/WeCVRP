using System.Globalization;
using WeCVRP.UI.Models;

namespace WeCVRP.UI.Converters;

public class AlgorithmEnumToNameConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not Algorithm algorithm)
            throw new ArgumentException($"\"{nameof(value)}\" must has \"{typeof(Algorithm).Name}\" type.", nameof(value));

        return algorithm switch
        {
            Algorithm.ClarkeWright => "Clarke-Wright",
            _ => Enum.GetName(algorithm) ?? throw new ArgumentException($"Unexpected enum value \"{algorithm}\".")
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
