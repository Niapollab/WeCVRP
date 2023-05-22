using Mapsui.UI.Maui;
using Mapsui;

namespace WeCVRP.UI.Extensions;

public static class LocationExtensions
{
    public static Position ToPosition(this Location location)
        => new (location.Latitude, location.Longitude);

    public static MPoint ToMPoint(this Location location)
        => location.ToPosition().ToMapsui();
}
