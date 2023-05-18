namespace WeCVRP.Core.Extensions;

public static class MatrixExtensions
{
    public static IEnumerable<(int RowIndex, int ColumnIndex, T Value)> Explode<T>(this T[,] matrix)
    {
        for (int i = 0; i < matrix.GetLength(0); ++i)
            for (int j = 0; j < matrix.GetLength(1); ++j)
                yield return (i, j, matrix[i, j]);
    }
}
