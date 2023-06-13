using WeCVRP.Algorithms;
using WeCVRP.Algorithms.Models;
using WeCVRP.Core;
using WeCVRP.Core.Models;

namespace WeCVRP.Service;

public class RemoteSolverService : IRemoteSolverService
{
    private readonly ICVRPCalculatorProvider _calculatorProvider;

    public RemoteSolverService(ICVRPCalculatorProvider calculatorProvider)
        => _calculatorProvider = calculatorProvider;

    public ValueTask<IReadOnlyList<Algorithm>> GetAlgorithmsAsync(CancellationToken cancellationToken = default)
        => ValueTask.FromResult((IReadOnlyList<Algorithm>)Enum.GetValues<Algorithm>());

    public async ValueTask<CVRPCalculationResponse> SolveAsync(Algorithm algorithm, CVRPCalculationRequest request, CancellationToken cancellationToken = default)
    {
        ICVRPCalculator? calculator = _calculatorProvider.GetByAlgorithm(algorithm);

        if (calculator is null)
            throw new ArgumentException($"Algorithm with name \"{algorithm}\" not found.", nameof(algorithm));

        return await calculator
            .CalculateAsync(request, cancellationToken)
            .ConfigureAwait(false);
    }
}
