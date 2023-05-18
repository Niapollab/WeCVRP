namespace WeCVRP.Core.Models;

public readonly record struct Point2D(double X, double Y)
{
    public static implicit operator Point2D((double X, double Y) point)
        => new Point2D(point.X, point.Y);
}
