using Microsoft.Maui.Maps;
using MapControl = Microsoft.Maui.Controls.Maps.Map;

namespace WeCVRP.UI.Extensions;

public static class MapExtensions
{
    public static async ValueTask<bool> TryToMoveToLocationAsync(this MapControl map, string locationName, Distance distance, CancellationToken cancellationToken = default)
    {
        try
        {
            IEnumerable<Location> locations = await Geocoding.Default.GetLocationsAsync(locationName);
            Location? location = locations.FirstOrDefault();

            if (location == null)
                return false;

            cancellationToken.ThrowIfCancellationRequested();

            map.MoveToRegion(MapSpan.FromCenterAndRadius(location, distance));
            return true;
        }
        catch
        {
            return false;
        }
    }
    public static async ValueTask<bool> TryToMoveToUserLocationAsync(this MapControl map, Distance distance, CancellationToken cancellationToken = default)
    {
        try
        {
            Location? location = await Geolocation.Default.GetLastKnownLocationAsync()
                ?? await Geolocation.Default.GetLocationAsync(new GeolocationRequest(), cancellationToken);

            if (location == null)
                return false;

            map.MoveToRegion(MapSpan.FromCenterAndRadius(location, distance));
            return true;
        }
        catch
        {
            return false;
        }
    }
}