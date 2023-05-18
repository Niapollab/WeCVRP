using WeCVRP.Core.Models;

namespace WeCVRP.Core;

public interface ICVRPCalculator
{
    ValueTask<CVRPCalculationResponse> CalculateAsync(CVRPCalculationRequest request, CancellationToken cancellationToken = default);
}
