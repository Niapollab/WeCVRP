using WeCVRP.Core.Models;

namespace WeCVRP.Core.Format;

public class TspLib95Serializer : ITspLib95Serializer
{
    public ValueTask<string> SerializeAsync(CVRPCalculationRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
