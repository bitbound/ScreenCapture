using Bitbound.ScreenCapture.Models;
using System.Diagnostics;
using System.Drawing.Imaging;

namespace Bitbound.ScreenCapture.Windows.Benchmarks;
public sealed class CaptureTests : IDisposable
{
    private readonly BitmapUtility _bitmapUtility;
    private readonly IScreenCapturer _capturer;
    private readonly IEnumerable<DisplayInfo> _displays;

    public CaptureTests()
    {
        _capturer = ScreenCapturer.CreateDefault();
        _bitmapUtility = new BitmapUtility();
        _displays = _capturer.GetDisplays();
    }

    public void Dispose()
    {
        _capturer.Dispose();
    }

    //[Benchmark(OperationsPerInvoke = 1)]
    public void DoCaptures()
    {
        var count = 300;
        var sw = Stopwatch.StartNew();
        var display1 = _displays.First(x => x.DeviceName == "\\\\.\\DISPLAY1");

        for (var i = 0; i < count; i++)
        {
            using var result = _capturer.Capture(display1, true, tryUseDirectX: true, allowFallbackToBitBlt: false);
        }

        var fps = Math.Round(count / sw.Elapsed.TotalSeconds);
        Console.WriteLine($"FPS: {fps}");
    }

    public void DoEncoding()
    {
        var count = 300;
        var display1 = _displays.First(x => x.DeviceName == "\\\\.\\DISPLAY1");
        using var result = _capturer.Capture(display1, true, tryUseDirectX: true, allowFallbackToBitBlt: true);
        if (!result.IsSuccess)
        {
            throw new Exception("Capture failed.");
        }

        var sw = Stopwatch.StartNew();
        for (var i = 0; i < count; i++)
        {
            _ = _bitmapUtility.Encode(result.Bitmap, ImageFormat.Jpeg);
        }

        var encodeTime = Math.Round(sw.Elapsed.TotalMilliseconds / count, 2);
        Console.WriteLine($"Encode Time: {encodeTime}ms");
    }
}
