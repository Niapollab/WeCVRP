﻿using Mapsui;
using Mapsui.UI;
using Mapsui.UI.Maui;
using WeCVRP.UI.Models;

namespace WeCVRP.UI.Controllers;

public class MapExtendedTapController : IDisposable
{
    private readonly TimeSpan _deltaMultiTap;

    private readonly TimeSpan _deltaLongTap;

    private readonly Timer _longTapTimer;

    private readonly Timer _singleTapTimer;

    private readonly double _deltaTapRadiusSquare;

    private DateTime? _lastTapTime;

    private MPoint? _lastTapPosition;

    private bool _disposedValue;

    public MapControl MapControl { get; }

    public event EventHandler<TapEventArgs>? SingleTap;

    public event EventHandler<TapEventArgs>? DoubleTap;

    public event EventHandler<TapEventArgs>? LongTap;

    public MapExtendedTapController(MapControl mapControl, TimeSpan deltaMultiTap, TimeSpan deltaLongTap, double deltaTapRadius)
    {
        MapControl = mapControl;
        MapControl.TouchStarted += OnTouchStarted;
        MapControl.TouchMove += OnTouchEnded;
        MapControl.TouchEnded += OnTouchEnded;

        _deltaMultiTap = deltaMultiTap;
        _deltaLongTap = deltaLongTap;
        _deltaTapRadiusSquare = deltaTapRadius * deltaTapRadius;

        _longTapTimer = new Timer(_ => LongTap?.Invoke(this, BuildEventArgs(_lastTapPosition!)));
        _singleTapTimer = new Timer(_ => SingleTap?.Invoke(this, BuildEventArgs(_lastTapPosition!)));

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
            MapControl.TouchStarted -= OnTouchStarted;
            MapControl.TouchMove -= OnTouchEnded;
            MapControl.TouchEnded -= OnTouchEnded;

            _longTapTimer.Dispose();
            _singleTapTimer.Dispose();
        }

        _disposedValue = true;
    }

    private void OnTouchStarted(object? sender, TouchedEventArgs eventArgs)
    {
        _longTapTimer.Change(_deltaLongTap, Timeout.InfiniteTimeSpan);
        _singleTapTimer.Change(_deltaMultiTap, Timeout.InfiniteTimeSpan);

        DateTime newTapTime = DateTime.Now;
        MPoint newTapPosition = eventArgs.ScreenPoints[^1];

        if (_lastTapPosition?.Distance(newTapPosition) < _deltaTapRadiusSquare
            && newTapTime - _lastTapTime <= _deltaMultiTap)
        {
            _singleTapTimer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
            DoubleTap?.Invoke(this, BuildEventArgs(newTapPosition));
        }

        _lastTapPosition = newTapPosition;
        _lastTapTime = newTapTime;
    }

    private void OnTouchEnded(object? sender, TouchedEventArgs eventArgs)
        => _longTapTimer.Change(Timeout.Infinite, Timeout.Infinite);

    private TapEventArgs BuildEventArgs(MPoint position)
        => new(position, MapControl.GetMapInfo(position));
}
