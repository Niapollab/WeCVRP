using WeCVRP.Algorithms.Models;
using WeCVRP.Core.Models;

namespace WeCVRP.Service;

public interface IRemoteSolverService
{
    ValueTask<IReadOnlyList<Algorithm>> GetAlgorithmsAsync(CancellationToken cancellationToken = default);

    ValueTask<CVRPCalculationResponse> SolveAsync(Algorithm algorithm, CVRPCalculationRequest request, CancellationToken cancellationToken = default);
}
