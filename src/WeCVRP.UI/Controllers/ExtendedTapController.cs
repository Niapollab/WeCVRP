using WeCVRP.UI.Models;

namespace WeCVRP.UI.Controllers;

public class ExtendedTapController
{
    public event EventHandler<TapEventArgs>? DoubleTap;

    public event EventHandler<TapEventArgs>? LongTap;

    private readonly TimeSpan _deltaMultiTap;

    private readonly TimeSpan _deltaLongTap;

    private readonly Timer _longTapTimer;

    private readonly double _deltaTapRadiusSquare;

    private DateTime? _lastTapTime;

    private Point? _lastTapPoint;

    public ExtendedTapController(TimeSpan deltaMultiTap, TimeSpan deltaLongTap, double deltaTapRadius)
    {
        _deltaMultiTap = deltaMultiTap;
        _deltaLongTap = deltaLongTap;
        _deltaTapRadiusSquare = deltaTapRadius * deltaTapRadius;

        _longTapTimer = new Timer(LongTapCallback);

        _lastTapTime = null;
        _lastTapPoint = null;
    }

    public void MouseDown(Point mousePosition)
    {
        _longTapTimer.Change(_deltaLongTap, Timeout.InfiniteTimeSpan);

        DateTime newTapTime = DateTime.Now;
        Point newPoint = mousePosition;

        if (_lastTapPoint?.Distance(newPoint) <= _deltaTapRadiusSquare && newTapTime - _lastTapTime <= _deltaMultiTap)
            DoubleTap?.Invoke(this, new TapEventArgs());

        _lastTapPoint = newPoint;
        _lastTapTime = newTapTime;
    }

    public void MouseUp()
        => _longTapTimer.Change(Timeout.Infinite, Timeout.Infinite);

    private void LongTapCallback(object? state)
        => LongTap?.Invoke(this, new TapEventArgs());
}
