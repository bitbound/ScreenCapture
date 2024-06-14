using System.Drawing;
using System.Numerics;
using Windows.Win32;

namespace Bitbound.ScreenCapture.Models;

public class DisplayInfo
{
    public required string DeviceName { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public nint Hmon { get; set; }
    public bool IsPrimary { get; set; }
    public Rectangle MonitorArea { get; set; }
    public Vector2 ScreenSize { get; set; }
    public Rectangle WorkArea { get; set; }
}
