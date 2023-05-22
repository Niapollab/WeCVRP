namespace WeCVRP.UI.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
        => InitializeComponent();

    protected async override void OnAppearing()
        => await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
}
