using System.Globalization;
using WeCVRP.UI.Models;

namespace WeCVRP.UI.Converters;

public class AlgorithmNameConverter : EnumNameConverter<Algorithm>
{
    public override string Convert(Algorithm value, object parameter, CultureInfo culture)
        => value switch
        {
            Algorithm.ClarkeWright => "Clarke-Wright",
            _ => base.Convert(value, parameter, culture)
        };

    public override Algorithm ConvertBack(string value, object parameter, CultureInfo culture)
        => value switch
        {
            "Clarke-Wright" => Algorithm.ClarkeWright,
            _ => base.ConvertBack(value, parameter, culture)
        };
}
