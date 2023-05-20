using WeCVRP.UI.Extensions;
using WeCVRP.UI.Views;

namespace WeCVRP.UI;

public partial class MainPage : ContentPage
{
    public MainPage()
        => InitializeComponent();

    protected async override void OnAppearing()
    {
        await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        await map.TryToMoveToUserLocationAsync(Constants.MapScale);

        ForceEnableZoomGestures();
    }

    private async void OnPlaceSearchEntryCompleted(object sender, EventArgs e)
        => await map.TryToMoveToLocationAsync(placeSearchEntry.Text, Constants.MapScale);

    private void OnPlusButtonClicked(object sender, EventArgs e)
    {
        var searchOptionDialog = new SettingsPage();
        searchOptionDialog.Show(Window);
    }

    private void ForceEnableZoomGestures()
        => map.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == "VisibleRegion" && !map.GetZoomGesturesEnabled())
                map.SetZoomGesturesEnabled(true);
        };
}
