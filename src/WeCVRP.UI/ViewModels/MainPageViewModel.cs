using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WeCVRP.UI.Models;
using WeCVRP.UI.Extensions;
using Map = Mapsui.Map;
using Mapsui;

namespace WeCVRP.UI.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    [ObservableProperty]
    private Algorithm _algorithm;

    [ObservableProperty]
    private Direction _direction;

    [ObservableProperty]
    private int _pointsCount;

    [ObservableProperty]
    private string? _placeName;

    [ObservableProperty]
    private Map _map = new();

    private Location _homeLocation;

    public MainPageViewModel()
    {
        // Location shown on map by default (if location service is not available)
        _homeLocation = new Location(20.798363, -156.331924);

        _map.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());
        _map.Home = n => n.CenterOnAndZoomTo(_homeLocation.ToMPoint(), n.Resolutions[10]);
    }

    [RelayCommand]
    private static Task OpenFileAsync()
        => Task.CompletedTask;

    [RelayCommand]
    private static Task SaveFileAsync()
        => Task.CompletedTask;

    [RelayCommand]
    private static Task SaveAsFileAsync()
        => Task.CompletedTask;

    [RelayCommand]
    private static void OpenAbout()
    {
    }

    [RelayCommand]
    private static void Exit()
        => Environment.Exit(0);

    [RelayCommand]
    private void AlgorithmClicked(string? value)
        => Algorithm = Enum.Parse<Algorithm>(value!);

    [RelayCommand]
    private void DirectionClicked(string? value)
        => Direction = Enum.Parse<Direction>(value!);

    [RelayCommand]
    private async Task SearchAsync()
    {
        if (!string.IsNullOrEmpty(PlaceName))
            await TryToMoveToLocationAsync(PlaceName);
    }

    public async ValueTask<bool> TryToMoveToUserLocationAsync(double? resolution = null, CancellationToken cancellationToken = default)
    {
        resolution ??= Map.Navigator.Resolutions[16];
        try
        {
            Location? location = await Geolocation.Default.GetLastKnownLocationAsync()
                ?? await Geolocation.Default.GetLocationAsync(new GeolocationRequest(), cancellationToken);

            if (location == null)
                return false;

            Navigator navigator = Map.Navigator;
            navigator.CenterOnAndZoomTo(location.ToMPoint(), resolution.Value);

            _homeLocation = location;

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async ValueTask<bool> TryToMoveToLocationAsync(string locationName, double? resolution = null, CancellationToken cancellationToken = default)
    {
        resolution ??= Map.Navigator.Resolutions[16];
        try
        {
            IEnumerable<Location> locations = await Geocoding.Default.GetLocationsAsync(locationName);
            Location? location = locations.FirstOrDefault();

            if (location == null)
                return false;

            cancellationToken.ThrowIfCancellationRequested();

            Navigator navigator = Map.Navigator;
            navigator.CenterOnAndZoomTo(location.ToMPoint(), resolution.Value);

            _homeLocation = location;

            return true;
        }
        catch
        {
            return false;
        }
    }
}
