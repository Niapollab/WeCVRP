using WeCVRP.Core;
using WeCVRP.Core.Format;
using WeCVRP.Core.Models;
using System.Text.Json;

namespace WeCVRP.UI;

public class RemoteCVRPCalculator : ICVRPCalculator
{
    private readonly ITspLib95Serializer _tspLib95Serializer;

    public string? AlgorithmName { get; set; }

    public string EndPoint { get; }

    public RemoteCVRPCalculator(string baseUrl, ITspLib95Serializer tspLib95Serializer)
    {
        _tspLib95Serializer = tspLib95Serializer;
        EndPoint = $"{baseUrl}/api/v1/solver";
    }

    public async ValueTask<CVRPCalculationResponse> CalculateAsync(CVRPCalculationRequest request, CancellationToken cancellationToken = default)
    {
        if (AlgorithmName is null)
            throw new NullReferenceException($"{nameof(AlgorithmName)} has null value.");

        using HttpContent content = await BuildContentAsync(request, cancellationToken)
            .ConfigureAwait(false);

        using var client = new HttpClient();
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"{EndPoint}/{AlgorithmName}")
        {
            Content = content
        };

        using HttpResponseMessage response = await client
            .SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        using Stream inputStream = await response
            .Content
            .ReadAsStreamAsync(cancellationToken)
            .ConfigureAwait(false);

        return await JsonSerializer
            .DeserializeAsync<CVRPCalculationResponse>(inputStream, cancellationToken: cancellationToken)
            .ConfigureAwait(false) ?? throw new ArgumentException($"Unable to parse as valid json.", nameof(request));
    }

    private async ValueTask<MultipartFormDataContent> BuildContentAsync(CVRPCalculationRequest request, CancellationToken cancellationToken)
    {
        var tsplib95 = new StringContent(await _tspLib95Serializer
            .SerializeAsync(request, cancellationToken)
            .ConfigureAwait(false));

        var content = new MultipartFormDataContent();
        content.Add(tsplib95, nameof(tsplib95));

        return content;
    }
}
