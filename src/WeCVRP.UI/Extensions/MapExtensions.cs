using Microsoft.Maui.Maps;
using Microsoft.Maui.Maps.Handlers;
using Map = Microsoft.Maui.Controls.Maps.Map;

namespace WeCVRP.UI.Extensions;

public static class MapExtensions
{
    public static async ValueTask<bool> TryToMoveToLocationAsync(this Map map, string locationName, Distance distance, CancellationToken cancellationToken = default)
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

    public static async ValueTask<bool> TryToMoveToUserLocationAsync(this Map map, Distance distance, CancellationToken cancellationToken = default)
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

    public static bool GetZoomControlsEnabled(this Map map)
    {
#if __ANDROID__
        return map.Handler is IMapHandler handler && handler.Map is not null
            ? handler.Map.UiSettings.ZoomControlsEnabled
            : map.IsZoomEnabled;
#else
        return map.IsZoomEnabled;
#endif
    }

    public static bool GetZoomGesturesEnabled(this Map map)
    {
#if __ANDROID__
        return map.Handler is IMapHandler handler && handler.Map is not null
            ? handler.Map.UiSettings.ZoomGesturesEnabled
            : map.IsZoomEnabled;
#else
        return map.IsZoomEnabled;
#endif
    }

    public static void SetZoomControlsEnabled(this Map map, bool enabled)
    {
#if __ANDROID__
        if (map.Handler is IMapHandler handler && handler.Map is not null)
            handler.Map.UiSettings.ZoomControlsEnabled = enabled;
#endif
    }

    public static void SetZoomGesturesEnabled(this Map map, bool enabled)
    {
#if __ANDROID__
        if (map.Handler is IMapHandler handler && handler.Map is not null)
            handler.Map.UiSettings.ZoomGesturesEnabled = enabled;
#endif
    }
}
