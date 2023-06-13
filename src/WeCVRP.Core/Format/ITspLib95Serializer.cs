namespace WeCVRP.Core.Format;

public interface ITspLib95Serializer
{
    ValueTask<string> SerializeAsync(CancellationToken cancellationToken = default);
}
