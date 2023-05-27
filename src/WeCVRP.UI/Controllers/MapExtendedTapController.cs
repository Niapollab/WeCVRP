using Mapsui;
using Mapsui.UI;
using Mapsui.UI.Maui;
using WeCVRP.UI.Models;

namespace WeCVRP.UI.Controllers;

public class MapExtendedTapController : IDisposable
{
    private readonly TimeSpan _deltaMultiTap;

    private readonly TimeSpan _deltaLongTap;

    private readonly Timer _longTapTimer;

    private readonly double _deltaTapRadiusSquare;

    private DateTime? _lastTapTime;

    private MPoint? _lastTapPosition;

    private bool _disposedValue;

    public MapControl MapControl { get; }

    public event EventHandler<TapEventArgs>? DoubleTap;

    public event EventHandler<TapEventArgs>? LongTap;

    public MapExtendedTapController(MapControl mapControl, TimeSpan deltaMultiTap, TimeSpan deltaLongTap, double deltaTapRadius)
    {
        MapControl = mapControl;
        MapControl.TouchStarted += HandleTouchStarted;
        MapControl.TouchMove += HandleTouchEnded;
        MapControl.TouchEnded += HandleTouchEnded;

        _deltaMultiTap = deltaMultiTap;
        _deltaLongTap = deltaLongTap;
        _deltaTapRadiusSquare = deltaTapRadius * deltaTapRadius;

        _longTapTimer = new Timer(_ => LongTap?.Invoke(this, BuildEventArgs(_lastTapPosition!)));

        _lastTapTime = null;
        _lastTapPosition = null;
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposedValue)
            return;

        if (disposing)
        {
            MapControl.TouchStarted -= HandleTouchStarted;
            MapControl.TouchMove -= HandleTouchEnded;
            MapControl.TouchEnded -= HandleTouchEnded;

            _longTapTimer.Dispose();
        }

        _disposedValue = true;
    }

    private void HandleTouchStarted(object? sender, TouchedEventArgs eventArgs)
    {
        _longTapTimer.Change(_deltaLongTap, Timeout.InfiniteTimeSpan);

        DateTime newTapTime = DateTime.Now;
        MPoint newTapPosition = eventArgs.ScreenPoints[0];

        if (_lastTapPosition?.Distance(newTapPosition) < _deltaTapRadiusSquare
            && newTapTime - _lastTapTime <= _deltaMultiTap)
            DoubleTap?.Invoke(this, BuildEventArgs(newTapPosition));

        _lastTapPosition = newTapPosition;
        _lastTapTime = newTapTime;
    }

    private void HandleTouchEnded(object? sender, TouchedEventArgs eventArgs)
        => _longTapTimer.Change(Timeout.Infinite, Timeout.Infinite);

    private TapEventArgs BuildEventArgs(MPoint position)
        => new(position, MapControl.GetMapInfo(position));
}
