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

        ForceEnableZoomGestures();
    }

    private async void OnPlaceSearchEntryCompleted(object sender, EventArgs e)
        => await map.TryToMoveToLocationAsync(placeSearchEntry.Text, Constants.MapScale);

    private void ForceEnableZoomGestures()
        => map.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == "VisibleRegion" && !map.GetZoomGesturesEnabled())
                map.SetZoomGesturesEnabled(true);
        };
}
