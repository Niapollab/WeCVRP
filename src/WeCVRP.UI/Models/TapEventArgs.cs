using Mapsui;

namespace WeCVRP.UI.Models;

public class TapEventArgs : EventArgs
{
    public MPoint Position { get; }

    public MapInfo? MapInfo { get; }

    public TapEventArgs(MPoint position, MapInfo? mapInfo)
    {
        Position = position;
        MapInfo = mapInfo;
    }
}
