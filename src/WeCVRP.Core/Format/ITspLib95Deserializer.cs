using WeCVRP.Core.Models;

namespace WeCVRP.Core.Format;

public interface ITspLib95Deserializer
{
    ValueTask<CVRPCalculationRequest> DeserializeAsync(string tspLib95, CancellationToken cancellationToken = default);
}
