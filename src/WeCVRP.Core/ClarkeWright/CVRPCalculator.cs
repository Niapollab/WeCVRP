using WeCVRP.Core.Extensions;
using WeCVRP.Core.Models;

namespace WeCVRP.Core.ClarkeWright;

public class CVRPCalculator : ICVRPCalculator
{
    protected struct RouteInfo
    {
        public List<int> Route { get; set; }

        public int TotalDemand { get; set; }
    }

    public ValueTask<CVRPCalculationResponse> CalculateAsync(CVRPCalculationRequest request, CancellationToken cancellationToken = default)
    {
        int size = request.AdjacencyMatrix.GetLength(0);
        int depot = request.Depot;
        List<RouteInfo> routeInfo = BuildInitialRoutes(size, depot, request.ClientDemands);
        double[,] savingsMatrix = CalculateSavingsMatrix(request.AdjacencyMatrix, request.Depot);
        IEnumerable<(int RowIndex, int ColumnIndex)> orderedSavingsIndexes = SortMatrixByAscending(savingsMatrix);

        foreach ((int i, int j) in orderedSavingsIndexes)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (routeInfo.Any(ri => ri.Route.Contains(i) && ri.Route.Contains(j)))
                continue;

            cancellationToken.ThrowIfCancellationRequested();
            IReadOnlyList<int> routesForMergeIndexes = routeInfo
                .Select((ri, i) => (r: ri.Route, i))
                .Where(p => (p.r[1] == i && p.r[^2] == j) || (p.r[^2] == i || p.r[1] == j))
                .Select(p => p.i)
                .ToArray();

            if (routesForMergeIndexes.Count < 2)
                continue;

            cancellationToken.ThrowIfCancellationRequested();
            (int FirstIndex, int SecondIndex)? pair = FindTwoSuitableRoutesIndexes(routeInfo, routesForMergeIndexes, request.TransportCapacity);

            if (pair is null)
                continue;

            // i must be last point (before depot) of first route
            // If it is not truth - swap indexes
            if (i != routeInfo[pair.Value.FirstIndex].Route[^2])
                pair = (pair.Value.SecondIndex, pair.Value.FirstIndex);

            cancellationToken.ThrowIfCancellationRequested();
            MergeRoutes(routeInfo, pair.Value.FirstIndex, pair.Value.SecondIndex);
        }

        IReadOnlyList<IReadOnlyList<int>> finalRoutes = routeInfo
            .Select(ri => ri.Route)
            .ToArray();
        return ValueTask.FromResult(new CVRPCalculationResponse(finalRoutes));
    }

    protected void MergeRoutes(List<RouteInfo> routeInfo, int firstIndex, int secondIndex)
    {
        RouteInfo first = routeInfo[firstIndex];
        RouteInfo second = routeInfo[secondIndex];

        List<int> result = first
            .Route
            .SkipLast(1)
            .Concat(second.Route.Skip(1))
            .ToList();

        routeInfo.RemoveAt(Math.Max(firstIndex, secondIndex));
        routeInfo.RemoveAt(Math.Min(firstIndex, secondIndex));

        routeInfo.Add(new RouteInfo
        {
            Route = result,
            TotalDemand = first.TotalDemand + second.TotalDemand
        });
    }

    protected (int FirstIndex, int secondIndex)? FindTwoSuitableRoutesIndexes(List<RouteInfo> routeInfos, IReadOnlyList<int> routesForMergeIndexes, int transportCapacity)
    {
        int size = routesForMergeIndexes.Count;

        for (int i = 0; i < size; ++i)
            for (int j = 0; j < size; ++j)
            {
                if (i == j)
                    continue;

                int firstIndex = routesForMergeIndexes[i];
                int secondIndex = routesForMergeIndexes[j];

                RouteInfo first = routeInfos[firstIndex];
                RouteInfo second = routeInfos[secondIndex];

                if (first.TotalDemand + second.TotalDemand <= transportCapacity)
                    return (firstIndex, secondIndex);
            }

        return null;
    }

    protected double[,] CalculateSavingsMatrix(double[,] adjacencyMatrix, int depot)
    {
        int size = adjacencyMatrix.GetLength(0);
        var savingsMatrix = new double[size, size];

        for (int i = 0; i < size; ++i)
            for (int j = 0; j < size; ++j)
                savingsMatrix[i, j] = i != j
                    ? adjacencyMatrix[depot, i] + adjacencyMatrix[depot, j] - adjacencyMatrix[i, j]
                    : savingsMatrix[i, j];

        return savingsMatrix;
    }

    protected virtual IEnumerable<(int RowIndex, int ColumnIndex)> SortMatrixByAscending(double[,] savingsMatrix)
        => savingsMatrix
            .Explode()
            .OrderBy(e => -e.Value)
            .Select(e => (e.RowIndex, e.ColumnIndex));

    protected List<RouteInfo> BuildInitialRoutes(int routesCount, int depot, IReadOnlyList<int> clientDemands)
        => Enumerable
            .Range(0, routesCount)
            .Where(i => i != depot)
            .Select(i => new RouteInfo
            {
                Route = new List<int> { depot, i, depot },
                TotalDemand = clientDemands[i]
            })
            .ToList();
}
