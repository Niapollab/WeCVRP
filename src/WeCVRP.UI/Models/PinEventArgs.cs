using Mapsui.UI.Maui;

namespace WeCVRP.UI.Models;

public class PinEventArgs : EventArgs
{
    public Position Position { get; }

    public bool IsDepot { get; }

    public PinEventArgs(Position position, bool isDepot)
    {
        Position = position;
        IsDepot = isDepot;
    }
}
