namespace WeCVRP.Core.Models;

public class CVRPCalculationRequest
{
    public double[,] AdjacencyMatrix { get; }

    public int Depot { get; }

    public IReadOnlyList<int> ClientDemands { get; }

    public int TransportCapacity { get; }

    public CVRPCalculationRequest(double[,] adjacencyMatrix, int depot, IReadOnlyList<int> clientDemands, int transportCapacity)
    {
        if (adjacencyMatrix.GetLength(0) != adjacencyMatrix.GetLength(1))
            throw new ArgumentException($"\"{nameof(adjacencyMatrix)}\" must be square matrix.", nameof(adjacencyMatrix));

        if (depot < 0 || depot >= adjacencyMatrix.GetLength(0))
            throw new ArgumentException($"\"{nameof(depot)}\" must be in range \"{nameof(adjacencyMatrix)}\" length.", nameof(depot));

        if (clientDemands.Count != adjacencyMatrix.GetLength(0))
            throw new ArgumentException($"\"{nameof(clientDemands)}\" length must be equal to matrix length.", nameof(clientDemands));

        if (clientDemands[depot] != 0)
            throw new ArgumentException($"\"{nameof(clientDemands)}\" must be equal 0 by depot-index \"{depot}\".", nameof(clientDemands));

        if (clientDemands.Any(cd => cd < 0))
            throw new ArgumentException($"All \"{nameof(clientDemands)}\" values must be equal or greater than zero.", nameof(clientDemands));

        if (clientDemands.Max() > transportCapacity)
            throw new ArgumentException($"Max \"{nameof(transportCapacity)}\" must be equals or greater than \"{nameof(transportCapacity)}\" value.", nameof(transportCapacity));

        AdjacencyMatrix = adjacencyMatrix;
        Depot = depot;
        ClientDemands = clientDemands;
        TransportCapacity = transportCapacity;
    }

    public static CVRPCalculationRequest FromCartesianCoordinateSystem(IReadOnlyList<Point2D> points, int depot, IReadOnlyList<int> clientDemands, int transportCapacity)
    {
        int size = points.Count;
        var adjacencyMatrix = new double[size, size];

        for (int i = 0; i < size; ++i)
            for (int j = 0; j < size; ++j)
            {
                Point2D first = points[i];
                Point2D second = points[j];

                double deltaX = first.X - second.X;
                double deltaY = first.Y - second.Y;

                adjacencyMatrix[i, j] = adjacencyMatrix[j, i] = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
            }

        return new CVRPCalculationRequest(adjacencyMatrix, depot, clientDemands, transportCapacity);
    }
}
