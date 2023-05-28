using Mapsui.UI.Maui;

namespace WeCVRP.UI.Models;

public class PinRemoveEventArgs : EventArgs
{
    public Pin Pin { get; }

    public PinRemoveEventArgs(Pin pin)
        => Pin = pin;
}
