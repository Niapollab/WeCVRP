using IResourceDictionary = Microsoft.Maui.Controls.Internals.IResourceDictionary;

namespace WeCVRP.UI.Extensions;

public static class ResourceDictionaryExtensions
{
    public static T? TryGet<T>(this IResourceDictionary resourceDictionary, string resourceName)
        => resourceDictionary.TryGetValue(resourceName, out var rawValue) && rawValue is T outValue
            ? outValue
            : default;

    public static T Get<T>(this IResourceDictionary resourceDictionary, string resourceName)
        => resourceDictionary.TryGet<T>(resourceName) ?? throw new ArgumentException($"Unable to find resource with name \"{resourceName}\".", nameof(resourceName));
}
