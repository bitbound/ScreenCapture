using Bitbound.ScreenCapture.Models;
using System.Diagnostics;
using System.Drawing.Imaging;

namespace Bitbound.ScreenCapture.Windows.Benchmarks;
public sealed class CaptureTests
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

    internal void DoCaptureEncodeAndDiff()
    {
        var count = 300;
        var sw = Stopwatch.StartNew();
        var display1 = _displays.First(x => x.DeviceName == "\\\\.\\DISPLAY1");
        CaptureResult? lastResult = null;
        byte[] bytes = [];

        for (var i = 0; i < count; i++)
        {
            var result = _capturer.Capture(display1, true, tryUseDirectX: true, allowFallbackToBitBlt: true);
            if (!result.IsSuccess)
            {
                continue;
            }

            try
            {
                var diffArea = _bitmapUtility.GetChangedArea(result.Bitmap, lastResult?.Bitmap);
                if (!diffArea.IsSuccess)
                {
                    continue;
                }

                if (diffArea.Value.IsEmpty)
                {
                    continue;
                }

                using var cropped = _bitmapUtility.CropBitmap(result.Bitmap, diffArea.Value);
                bytes = _bitmapUtility.Encode(cropped, ImageFormat.Jpeg);

            }
            finally
            {
                lastResult?.Dispose();
                lastResult = result;
            }
        }

        var fps = Math.Round(count / sw.Elapsed.TotalSeconds);
        Console.WriteLine($"FPS: {fps}");
    }
}
