using WeCVRP.UI.Models;

namespace WeCVRP.UI.ViewModels;

public class SettingsViewModel : BindableObject
{
    public static readonly BindableProperty AlgorithmProperty = BindableProperty.Create(nameof(Algorithm), typeof(Algorithm), typeof(SettingsViewModel), Algorithm.ClarkeWright);

    public static readonly BindableProperty DirectionProperty = BindableProperty.Create(nameof(Direction), typeof(Direction), typeof(SettingsViewModel), Direction.Relative);

    public Algorithm Algorithm
    {
        get => (Algorithm)GetValue(AlgorithmProperty);
        set => SetValue(AlgorithmProperty, value);
    }

    public Direction Direction
    {
        get => (Direction)GetValue(DirectionProperty);
        set => SetValue(DirectionProperty, value);
    }
}
