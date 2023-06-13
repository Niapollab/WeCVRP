using WeCVRP.Algorithms.Models;
using WeCVRP.Core;
using ClarkeWrightCVRPCalculator = WeCVRP.Algorithms.ClarkeWright.CVRPCalculator;

namespace WeCVRP.Algorithms;

public class CVRPCalculatorProvider : ICVRPCalculatorProvider
{
    public ICVRPCalculator? GetByAlgorithm(Algorithm algorithm)
        => algorithm switch
        {
            Algorithm.ClarkeWright => new ClarkeWrightCVRPCalculator(),
            _ => null
        };
}
