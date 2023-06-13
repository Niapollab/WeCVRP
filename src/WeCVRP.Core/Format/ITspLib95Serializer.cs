using WeCVRP.Core.Models;

namespace WeCVRP.Core.Format;

public interface ITspLib95Serializer
{
    ValueTask<string> SerializeAsync(CVRPCalculationRequest request, CancellationToken cancellationToken = default);
}
