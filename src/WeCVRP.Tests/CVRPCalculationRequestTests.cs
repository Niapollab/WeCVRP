namespace WeCVRP.Tests;

public class CVRPCalculationRequestTests
{
    [Fact]
    public void MatrixMustBeSquare()
    {
        // arrange & act & assert
        Assert.Throws<ArgumentException>(() => new CVRPCalculationRequest(new double[,]
        {
            { 1.0, 3.0, 2.0 },
            { 1.0, 2.0, 3.0 }
        }, 0, new int[]
        {
            0,
            200
        }, 1000));
    }

    [Fact]
    public void DepotMustBeInMatrixRange()
    {
        // arrange & act & assert
        Assert.Throws<ArgumentException>(() => new CVRPCalculationRequest(new double[,]
        {
            { 1.0, 3.0 },
            { 1.0, 2.0 }
        }, 3, new int[]
        {
            0,
            200
        }, 1000));
    }

    [Fact]
    public void ClientsCountMustBeEqualsThanMatixLength()
    {
        // arrange & act & assert
        Assert.Throws<ArgumentException>(() => new CVRPCalculationRequest(new double[,]
        {
            { 1.0, 3.0 },
            { 1.0, 2.0 }
        }, 0, new int[]
        {
            0,
            200,
            300
        }, 1000));
    }

    [Fact]
    public void DepotDemandMustBeEqualsZero()
    {
        // arrange & act & assert
        Assert.Throws<ArgumentException>(() => new CVRPCalculationRequest(new double[,]
        {
            { 1.0, 3.0 },
            { 1.0, 2.0 }
        }, 0, new int[]
        {
            100,
            200
        }, 1000));
    }

    [Fact]
    public void AllClientDemandsMustBePositiveOrZero()
    {
        // arrange & act & assert
        Assert.Throws<ArgumentException>(() => new CVRPCalculationRequest(new double[,]
        {
            { 1.0, 3.0 },
            { 1.0, 2.0 }
        }, 0, new int[]
        {
            0,
            -200
        }, 1000));
    }

    [Fact]
    public void TransportCapacityMustBeGreaterThanMaxClientDemand()
    {
        // arrange & act & assert
        Assert.Throws<ArgumentException>(() => new CVRPCalculationRequest(new double[,]
        {
            { 1.0, 3.0 },
            { 1.0, 2.0 }
        }, 0, new int[]
        {
            0,
            1001
        }, 1000));
    }
}
