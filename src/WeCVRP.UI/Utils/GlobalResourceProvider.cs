namespace WeCVRP.UI.Utils;

public static class GlobalResourceProvider
{
    public static T? TryGet<T>(string resourceName)
        => (Application.Current?.Resources.TryGetValue(resourceName, out var rawValue) ?? false) && (rawValue is T outValue)
            ? outValue
            : default;

    public static T Get<T>(string resourceName)
        => TryGet<T>(resourceName) ?? throw new ArgumentException($"Unable to find resource with name \"{resourceName}\".", nameof(resourceName));
}
