using Mapsui;
using Mapsui.UI.Maui;
using WeCVRP.UI.Controllers;
using WeCVRP.UI.Extensions;

namespace WeCVRP.UI.Views;

public partial class MainPage : ContentPage
{
    private Location _homeLocation;

    private readonly SearchLocationController _searchLocationController;

    private readonly PinsController _pinsController;

    private Pin? _depot;

    public MainPage()
    {
        InitializeComponent();

        // Location shown on map by default (if location service is not available)
        _homeLocation = new Location(20.798363, -156.331924);

        _searchLocationController = new SearchLocationController();

        mapView.MyLocationLayer.Enabled = false;
        mapView.Map.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());
        mapView.Map.Home = n => n.CenterOnAndZoomTo(_homeLocation.ToMPoint(), n.Resolutions[10]);
        mapView.MyLocationLayer.Enabled = false;

        _pinsController = new PinsController(mapView, () => _depot, TimeSpan.FromMilliseconds(250), TimeSpan.FromMilliseconds(250), 5);
        _pinsController.PinAdd += OnPinAdd;
        _pinsController.PinRemove += OnPinRemove;
        _pinsController.PinMoveStarted += OnPinMoveStarted;
        _pinsController.PinMove += OnPinMove;
        _pinsController.PinMoveEnded += OnPinMoveEnded;
        _pinsController.PinDepotChanged += OnPinDepotChanged;
    }

    private void OnPinDepotChanged(object? sender, Models.PinDepotChangedEventArgs eventArgs)
    {
        _depot = eventArgs.NewDepot;

        eventArgs.NewDepot.Color = Resources.Get<Color>("pinDepotColor");

        if (eventArgs.OldDepot is not null)
            eventArgs.OldDepot.Color = Resources.Get<Color>("pinClientColor");
    }

    private void OnPinMoveEnded(object? sender, Models.PinMoveEventArgs eventArgs)
        => eventArgs.Pin.Color = eventArgs.Pin == _depot
            ? Resources.Get<Color>("pinDepotColor")
            : Resources.Get<Color>("pinClientColor");

    private void OnPinMoveStarted(object? sender, Models.PinMoveEventArgs eventArgs)
        => eventArgs.Pin.Color = eventArgs.Pin == _depot
            ? Resources.Get<Color>("pinDepotMoveColor")
            : Resources.Get<Color>("pinClientMoveColor");

    private void OnPinMove(object? sender, Models.PinMoveEventArgs eventArgs)
        => eventArgs.Pin.Position = eventArgs.NewPosition;

    private void OnPinRemove(object? sender, Models.PinRemoveEventArgs eventArgs)
    {
        mapView.Pins.Remove(eventArgs.Pin);

        if (eventArgs.Pin != _depot)
            return;

        _depot = null;

        if (mapView.Pins.Count < 1)
            return;

        Pin newDepot = mapView.Pins[0];
        _depot = newDepot;
        _depot.Color = Resources.Get<Color>("pinDepotColor");
    }

    private void OnPinAdd(object? sender, Models.PinAddEventArgs e)
    {
        Pin newPin = new()
        {
            Label = string.Empty,
            Position = e.Position
        };

        if (_depot is null)
        {
            newPin.Color = Resources.Get<Color>("pinDepotColor");
            _depot = newPin;
        }
        else
            newPin.Color = Resources.Get<Color>("pinClientColor");

        mapView.Pins.Add(newPin);
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
