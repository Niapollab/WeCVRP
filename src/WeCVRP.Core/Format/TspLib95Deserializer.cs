using WeCVRP.Core.Models;

namespace WeCVRP.Core.Format;

public class TspLib95Deserializer : ITspLib95Deserializer
{
    public ValueTask<CVRPCalculationRequest> DeserializeAsync(string tspLib95, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
