using Bitbound.ScreenCapture.Models;
using System.Diagnostics;

namespace Bitbound.ScreenCapture.Windows.Benchmarks;
public class CaptureTests
{
    private readonly IScreenCapturer _capturer;
    private readonly IEnumerable<DisplayInfo> _displays;

    public CaptureTests()
    {
        _capturer = ScreenCapturer.CreateDefault();
        _displays = _capturer.GetDisplays();
    }

    //[Benchmark(OperationsPerInvoke = 1)]
    public void DoCaptures()
    {
        var count = 300;
        var sw = Stopwatch.StartNew();

        for (var i = 0; i < count; i++)
        {
            using var result = _capturer.Capture(_displays.First(), true, tryUseDirectX: true, allowFallbackToBitBlt: false);
        }

        var fps = Math.Round(count / sw.Elapsed.TotalSeconds);
        Console.WriteLine($"FPS: {fps}");
    }
}
