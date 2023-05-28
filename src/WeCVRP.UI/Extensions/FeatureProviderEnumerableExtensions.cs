using Mapsui;
using Mapsui.UI.Objects;

namespace WeCVRP.UI.Extensions;

public static class FeatureProviderEnumerableExtensions
{
    public static T? FindByFeature<T>(this IEnumerable<T> featureProviders, IFeature feature) 
        where T: IFeatureProvider
        => featureProviders.FirstOrDefault(p => p.Feature == feature);
}
