using Mapsui.UI.Maui;

namespace WeCVRP.UI.Models;

public class PinRemoveEventArgs : EventArgs
{
    public Pin Pin { get; }

    public bool IsDepot { get; }

    public PinRemoveEventArgs(Pin pin, bool isDepot)
    {
        Pin = pin;
        IsDepot = isDepot;
    }
}
