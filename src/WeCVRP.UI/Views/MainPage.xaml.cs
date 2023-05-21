using Mapsui.UI.Maui;

namespace WeCVRP.UI.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        map.Map?.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());
    }

    protected async override void OnAppearing()
        => await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

    private void OnPlaceSearchEntryCompleted(object sender, EventArgs e)
        => throw new NotImplementedException();

    private void OnPlusButtonClicked(object sender, EventArgs e)
        => new SettingsPage
        {
            BindingContext = settingsViewModel
        }.Show(Window);
}
