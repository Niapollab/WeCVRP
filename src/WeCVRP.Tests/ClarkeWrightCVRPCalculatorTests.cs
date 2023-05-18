namespace WeCVRP.Tests;

public class ClarkeWrightCVRPCalculatorTests
{
    [Fact]
    public async void SuccessfulSolution()
    {
        // arrange
        var request = CVRPCalculationRequest.FromCartesianCoordinateSystem(new Point2D[]
        {
            (10.0, 15.0),
            (17.0, 15.0),
            (6.0, 15.0),
            (13.0, 3.0),
            (9.0, 20.0),
            (19.0, 7.0),
            (8.0, 8.0),
            (4.0, 14.0),
            (17.0, 2.0),
            (12.0, 22.0),
            (6.0, 12.0),
            (19.0, 17.0),
            (12.0, 8.0)
        }, 0, new int[]
        {
            0,
            450,
            400,
            400,
            200,
            150,
            450,
            250,
            200,
            450,
            300,
            475,
            550
        }, 1500);
        var calculator = new ClarkeWrightCVRPCalculator();
        CVRPCalculationResponse expected = new CVRPCalculationResponse(new List<IReadOnlyList<int>>
        {
            new List<int>
            {
               0, 5, 8, 3, 12, 0
            },
            new List<int>
            {
                0, 1, 11, 0
            },
            new List<int>
            {
                0, 9, 4, 0
            },
            new List<int>
            {
                0, 6, 10, 7, 2, 0
            }
        });

        // act
        CVRPCalculationResponse actual = await calculator.CalculateAsync(request);

        // assert
        Assert.Equal(expected, actual);
    }
}
