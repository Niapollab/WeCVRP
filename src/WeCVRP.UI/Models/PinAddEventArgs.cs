using Mapsui.UI.Maui;

namespace WeCVRP.UI.Models;

public class PinAddEventArgs : EventArgs
{
    public Position Position { get; }

    public bool IsDepot { get; }

    public PinAddEventArgs(Position position, bool isDepot)
    {
        Position = position;
        IsDepot = isDepot;
    }
}
