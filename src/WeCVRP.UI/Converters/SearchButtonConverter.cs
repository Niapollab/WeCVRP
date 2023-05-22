using System.Globalization;

namespace WeCVRP.UI.Converters;

public class SearchButtonConverter : ValueConverter<bool, string>
{
    public override string Convert(bool value, object parameter, CultureInfo culture)
        => value ? "Next" : "Search";

    public override bool ConvertBack(string value, object parameter, CultureInfo culture)
        => value == "Next";
}
