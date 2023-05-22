using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WeCVRP.UI.Models;
using Map = Mapsui.Map;

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

    public MainPageViewModel()
        => _map.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());

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
    private static Task SearchAsync()
        => Task.CompletedTask;
}
