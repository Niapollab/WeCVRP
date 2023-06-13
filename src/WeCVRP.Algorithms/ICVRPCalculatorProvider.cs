using WeCVRP.Algorithms.Models;
using WeCVRP.Core;

namespace WeCVRP.Algorithms;

public interface ICVRPCalculatorProvider
{
    ICVRPCalculator? GetByAlgorithm(Algorithm algorithm);
}
