using System.Drawing;
using Windows.Win32.Graphics.Direct3D11;
using Windows.Win32.Graphics.Dxgi;
using Windows.Win32.Graphics.Dxgi.Common;

namespace Bitbound.ScreenCapture.Models;
internal sealed class DxOutput : IDisposable
{
    public DxOutput(
        string deviceName,
        Rectangle bounds,
        IDXGIAdapter1 adapter,
        ID3D11Device device,
        ID3D11DeviceContext deviceContext,
        IDXGIOutputDuplication outputDuplication,
        DXGI_MODE_ROTATION rotation)
    {
        DeviceName = deviceName;
        Bounds = bounds;
        Adapter = adapter;
        Device = device;
        DeviceContext = deviceContext;
        OutputDuplication = outputDuplication;
        Rotation = rotation;
    }

    public IDXGIAdapter1 Adapter { get; }
    public string DeviceName { get; }
    public Rectangle Bounds { get; }
    public ID3D11Device Device { get; }
    public ID3D11DeviceContext DeviceContext { get; }
    public IDXGIOutputDuplication OutputDuplication { get; }
    public DXGI_MODE_ROTATION Rotation { get; }

    public void Dispose()
    {
        try
        {
            OutputDuplication.ReleaseFrame();
        }
        catch { }
        GC.SuppressFinalize(this);
    }
}