using Mapsui.UI.Maui;

namespace WeCVRP.UI.Models;

public class PinMoveEventArgs : EventArgs
{
    public Pin Pin { get; }

    public Position NewPosition { get; }

    public PinMoveEventArgs(Pin pin, Position newPosition)
    {
        Pin = pin;
        NewPosition = newPosition;
    }
}
