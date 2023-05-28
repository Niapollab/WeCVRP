using Mapsui;
using Mapsui.UI;
using Mapsui.UI.Maui;
using Mapsui.UI.Maui.Extensions;
using WeCVRP.UI.Extensions;
using WeCVRP.UI.Models;

namespace WeCVRP.UI.Controllers;

public class PinsController : IDisposable
{
    private readonly Func<Pin?> _depotGetter;

    private readonly MapExtendedTapController _mouseController;

    private bool _disposedValue;

    private Pin? _draggedPin;

    public event EventHandler<PinAddEventArgs>? PinAdd;

    public event EventHandler<PinRemoveEventArgs>? PinRemove;

    public event EventHandler<PinDepotChangedEventArgs>? PinDepotChanged;

    public event EventHandler<PinMoveEventArgs>? PinMoveStarted;

    public event EventHandler<PinMoveEventArgs>? PinMove;

    public event EventHandler<PinMoveEventArgs>? PinMoveEnded;

    public MapView MapView { get; }

    public PinsController(MapView mapView, Func<Pin?> depotGetter, TimeSpan deltaMultiTap, TimeSpan deltaLongTap, double deltaTapRadius)
    {
        _depotGetter = depotGetter;

        MapView = mapView;
        MapView.TouchMove += HandleTouchMove;
        MapView.TouchEnded += HandleTouchEnded;

        _mouseController = new MapExtendedTapController(MapView, deltaMultiTap, deltaLongTap, deltaTapRadius);
        _mouseController.SingleTap += HandleSingleTap;
        _mouseController.LongTap += HandleLongTap;
        _mouseController.DoubleTap += HandleDoubleTap;
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
            return;

        if (disposing)
        {
            MapView.TouchMove -= HandleTouchMove;
            MapView.TouchEnded -= HandleTouchEnded;

            _mouseController.LongTap -= HandleLongTap;
            _mouseController.DoubleTap -= HandleDoubleTap;
            _mouseController.SingleTap -= HandleSingleTap;
        }

        _disposedValue = true;
    }

    private void HandleSingleTap(object? sender, TapEventArgs eventArgs)
    {
        IFeature? feature = eventArgs.MapInfo?.Feature;

        if (feature is null)
            return;

        Pin? pin = MapView.Pins.FindByFeature(feature);

        if (pin is null || pin == _depotGetter())
            return;

        PinDepotChanged?.Invoke(this, new PinDepotChangedEventArgs(_depotGetter(), pin));
    }

    private void HandleTouchMove(object? sender, TouchedEventArgs eventArgs)
    {
        Position? position = MapView
            .GetMapInfo(eventArgs.ScreenPoints[0])
            ?.WorldPosition
            ?.ToNative();

        if (PinMove is null || position is null || _draggedPin is null)
            return;

        eventArgs.Handled = true;
        PinMove(this, new PinMoveEventArgs(_draggedPin, position.Value));
    }

    private void HandleTouchEnded(object? sender, TouchedEventArgs eventArgs)
    {
        Pin? pin = _draggedPin;
        _draggedPin = null;

        if (PinMoveEnded is null || pin is null)
            return;

        PinMoveEnded(this, new PinMoveEventArgs(pin, pin.Position));
    }

    private void HandleLongTap(object? sender, TapEventArgs eventArgs)
    {
        if (_draggedPin is not null)
            return;

        IFeature? feature = eventArgs.MapInfo?.Feature;

        if (feature is null)
            return;

        Pin? pin = MapView.Pins.FindByFeature(feature);

        if (pin is null)
            return;

        _draggedPin = pin;
        PinMoveStarted?.Invoke(this, new PinMoveEventArgs(pin, pin.Position));
    }

    private void HandleDoubleTap(object? sender, TapEventArgs eventArgs)
    {
        Position? position = eventArgs.MapInfo?.WorldPosition?.ToNative();

        if (position is null)
            return;

        IFeature? feature = eventArgs.MapInfo?.Feature;
        if (feature is not null)
        {
            Pin? pin = MapView.Pins.FindByFeature(feature);

            if (pin is not null)
                PinRemove?.Invoke(this, new PinRemoveEventArgs(pin));
        }
        else
            PinAdd?.Invoke(this, new PinAddEventArgs(position.Value));
    }
}
