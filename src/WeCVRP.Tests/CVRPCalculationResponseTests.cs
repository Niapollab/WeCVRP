namespace WeCVRP.Tests;

public class CVRPCalculationResponseTests
{
    [Fact]
    public void RequestSizeMustBeEqualsToMaximumRoutePointIndex()
    {
        // arrange
        var request = new CVRPCalculationRequest(new double[,]
        {
            { 1.0, 3.0 },
            { 1.0, 2.0 }
        }, 0, new int[]
        {
            0,
            200
        }, 1000);
        var response = new CVRPCalculationResponse(new List<IReadOnlyList<int>>
        {
            new List<int>
            {
                0, 1, 2
            }
        });

        // act & assert
        Assert.Throws<ArgumentException>(() => response.CalculatePrices(request));
    }

    [Fact]
    public void RoutesEqualsWithReverseWays()
    {
        // arrange
        var first = new CVRPCalculationResponse(new List<IReadOnlyList<int>>
        {
            new List<int>
            {
                1, 2, 3
            },
            new List<int>
            {
                4, 5, 6, 7
            }
        });
        var second = new CVRPCalculationResponse(new List<IReadOnlyList<int>>
        {
            new List<int>
            {
                7, 6, 5, 4
            },
            new List<int>
            {
                3, 2, 1
            }
        });

        // act & assert
        Assert.Equal(first, second);
    }
}
