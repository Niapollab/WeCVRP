using Microsoft.AspNetCore.Mvc;
using WeCVRP.Algorithms.Models;
using WeCVRP.Core.Format;
using WeCVRP.Core.Models;

namespace WeCVRP.Service.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class SolverController : ControllerBase
{
    private readonly IRemoteSolverService _remoteSolverService;

    private readonly ITspLib95Deserializer _tspLib95Deserializer;

    private readonly ILogger<SolverController> _logger;

    public SolverController(IRemoteSolverService remoteSolverService, ITspLib95Deserializer tspLib95Deserializer, ILogger<SolverController> logger)
    {
        _remoteSolverService = remoteSolverService;
        _tspLib95Deserializer = tspLib95Deserializer;
        _logger = logger;
    }

    [HttpGet("get-all")]
    public async ValueTask<IResult> GetAlgorithms(CancellationToken cancellationToken = default)
    {
        IReadOnlyList<Algorithm> algorithms = await _remoteSolverService
            .GetAlgorithmsAsync(cancellationToken)
            .ConfigureAwait(false);

        return Results.Json(algorithms.Select(a => Enum.GetName(a)));
    }

    [HttpPost("{algorithmName}")]
    public async ValueTask<IResult> Solve(string algorithmName, [FromForm] string tsplib95, CancellationToken cancellationToken = default)
    {
        try
        {
            Algorithm algorithm = Enum.Parse<Algorithm>(algorithmName);

            CVRPCalculationRequest request = await _tspLib95Deserializer
                .DeserializeAsync(tsplib95, cancellationToken)
                .ConfigureAwait(false);

            var response = await _remoteSolverService
                .SolveAsync(algorithm, request, cancellationToken)
                .ConfigureAwait(false);

            return Results.Json(response);
        }
        catch
        {
            return Results.BadRequest();
        }
    }
}
