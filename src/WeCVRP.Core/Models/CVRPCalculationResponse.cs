namespace WeCVRP.Core.Models;

public class CVRPCalculationResponse
{
    public IReadOnlyList<IReadOnlyList<int>> Routes { get; }

    public CVRPCalculationResponse(IReadOnlyList<IReadOnlyList<int>> routes)
        => Routes = routes;

    public double CalculateTotalPrice(CVRPCalculationRequest request)
        => CalculatePrices(request).Sum();

    public IReadOnlyList<double> CalculatePrices(CVRPCalculationRequest request)
    {
        int size = request.AdjacencyMatrix.GetLength(0);

        if (Routes.SelectMany(r => r).Any(v => v >= size))
            throw new ArgumentException($"All routes indexes must be equal or less than matrix length.", nameof(request));

        return Routes
            .Select(r => CalculateRoutePrice(request.AdjacencyMatrix, r))
            .ToArray();
    }

    public bool Equals(CVRPCalculationResponse? other)
        => other is not null
            && Routes.Count == other.Routes.Count
            && RoutesToHashSet().SetEquals(other.RoutesToHashSet());

    public override bool Equals(object? obj)
        => Equals(obj as CVRPCalculationResponse);

    public override int GetHashCode()
        => Routes.Count.GetHashCode();

    private double CalculateRoutePrice(double[,] adjacencyMatrix, IReadOnlyList<int> Route)
    {
        double sum = 0.0;

        for (int i = 0; i < Route.Count - 1; ++i)
            sum += adjacencyMatrix[Route[i], Route[i + 1]];

        return sum;
    }

    private IReadOnlySet<string> RoutesToHashSet()
    {
        const string SpaceSymbol = " ";
        var hashSet = new HashSet<string>();

        foreach (IReadOnlyList<int> route in Routes)
        {
            hashSet.Add(string.Join(SpaceSymbol, route));
            hashSet.Add(string.Join(SpaceSymbol, route.Reverse()));
        }

        return hashSet;
    }
}
