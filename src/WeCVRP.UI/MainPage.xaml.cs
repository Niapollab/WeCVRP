using WeCVRP.UI.Extensions;

namespace WeCVRP.UI;

public partial class MainPage : ContentPage
{
    public MainPage()
        => InitializeComponent();

    protected async override void OnAppearing()
    {
        await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        await map.TryToMoveToUserLocationAsync(Constants.MapScale);
    }

    private async void OnPlaceSearchEntryCompleted(object sender, EventArgs e)
        => await map.TryToMoveToLocationAsync(placeSearchEntry.Text, Constants.MapScale);
}