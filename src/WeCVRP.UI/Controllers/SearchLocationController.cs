namespace WeCVRP.UI.Controllers;

public class SearchLocationController
{
    private int _currentIndex = -1;

    private IReadOnlyList<Location> _locations = Array.Empty<Location>();

    public bool IsEmpty => _locations.Count == 0;

    public async ValueTask<bool> TryUpdateAsync(string newLocationName, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            _locations = (await Geocoding.Default.GetLocationsAsync(newLocationName))?.ToArray() ?? Array.Empty<Location>();
            _currentIndex = _locations.Count > 0 ? 0 : -1;

            return true;
        }
        catch
        {
            return false;
        }
    }

    public void Clear()
    {
        _currentIndex = -1;
        _locations = Array.Empty<Location>();
    }

    public Location? GetNextLocation()
    {
        if (_currentIndex < 0)
            return null;

        Location location = _locations[_currentIndex++];
        _currentIndex %= _locations.Count;

        return location;
    }
}
