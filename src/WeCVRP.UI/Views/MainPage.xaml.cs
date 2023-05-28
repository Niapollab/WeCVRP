using Mapsui;
using WeCVRP.UI.Controllers;
using WeCVRP.UI.Extensions;

namespace WeCVRP.UI.Views;

public partial class MainPage : ContentPage
{
    private Location _homeLocation;

    private readonly SearchLocationController _searchLocationController;

    public MainPage()
    {
        InitializeComponent();

        // Location shown on map by default (if location service is not available)
        _homeLocation = new Location(20.798363, -156.331924);

        _searchLocationController = new SearchLocationController();
        mapView.MyLocationLayer.Enabled = false;

        mapView.Map.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());
        mapView.Map.Home = n => n.CenterOnAndZoomTo(_homeLocation.ToMPoint(), n.Resolutions[10]);
    }

    protected async override void OnAppearing()
    {
        await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        await TryToMoveToUserLocationAsync();
    }

    private void OnSearchTextChanged(object? sender, EventArgs eventArgs)
        => DropLocations();

    private void OnOpenFileClicked(object sender, EventArgs eventArgs)
    {
        // TODO
    }

    private void OnSaveFileClicked(object sender, EventArgs eventArgs)
    {
        // TODO
    }

    private void OnSaveAsFileClicked(object sender, EventArgs eventArgs)
    {
        // TODO
    }

    private void OnOpenAboutClicked(object sender, EventArgs eventArgs)
    {
        // TODO
    }

    private void OnExitClicked(object sender, EventArgs eventArgs)
        => Environment.Exit(0);

    private async void OnSearch(object sender, EventArgs eventArgs)
    {
        if (string.IsNullOrEmpty(mainPageViewModel.PlaceName))
        {
            mainPageViewModel.HasFoundLocations = false;
            return;
        }

        Location? location = _searchLocationController.GetNextLocation();

        if (location is null)
        {
            await _searchLocationController.TryUpdateAsync(mainPageViewModel.PlaceName);
            location = _searchLocationController.GetNextLocation();

            mainPageViewModel.HasFoundLocations = location is not null;

            if (location is null)
                return;
        }

        Navigator navigator = mapView.Map.Navigator;
        navigator.CenterOnAndZoomTo(location.ToMPoint(), mapView.Map.Navigator.Resolutions[16]);
    }

    private void DropLocations()
    {
        mainPageViewModel.HasFoundLocations = false;
        _searchLocationController.Clear();
    }

    private async ValueTask<bool> TryToMoveToUserLocationAsync(double? resolution = null, CancellationToken cancellationToken = default)
    {
        resolution ??= mapView.Map.Navigator.Resolutions[16];
        try
        {
            Location? location = await Geolocation.Default.GetLastKnownLocationAsync()
                ?? await Geolocation.Default.GetLocationAsync(new GeolocationRequest(), cancellationToken);

            if (location == null)
                return false;

            Navigator navigator = mapView.Map.Navigator;
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
