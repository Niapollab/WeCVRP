using Mapsui.UI.Maui;

namespace WeCVRP.UI.Models;

public class PinDepotChangedEventArgs : EventArgs
{
    public Pin? OldDepot { get; }

    public Pin NewDepot { get; }

    public PinDepotChangedEventArgs(Pin? oldDepot, Pin newDepot)
    {
        OldDepot = oldDepot;
        NewDepot = newDepot;
    }
}
