using Mapsui.UI.Maui;

namespace WeCVRP.UI.Models;

public class PinAddEventArgs : EventArgs
{
    public Position Position { get; }

    public PinAddEventArgs(Position position)
        => Position = position;
}
