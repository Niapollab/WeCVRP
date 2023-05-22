namespace WeCVRP.UI.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
        => InitializeComponent();

    protected async override void OnAppearing()
    {
        await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        await mainPageViewModel.TryToMoveToUserLocationAsync();
    }

    private void OnSearchTextChanged(object? sender, EventArgs eventArgs)
        => mainPageViewModel.DropLocations();
}
