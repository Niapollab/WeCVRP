namespace WeCVRP.UI.Utils;

public static class ResourceProvider
{
    public static T? Get<T>(string resourceName)
        => (Application.Current?.Resources.TryGetValue(resourceName, out var rawValue) ?? false) && (rawValue is T outValue)
            ? outValue
            : default;
}
