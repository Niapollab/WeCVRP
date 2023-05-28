using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WeCVRP.UI.Models;

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
    private bool _hasFoundLocations;

    [RelayCommand]
    private void AlgorithmClicked(string? value)
        => Algorithm = Enum.Parse<Algorithm>(value!);

    [RelayCommand]
    private void DirectionClicked(string? value)
        => Direction = Enum.Parse<Direction>(value!);
}
