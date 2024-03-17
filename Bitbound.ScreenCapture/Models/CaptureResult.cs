using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using Windows.Win32.Foundation;

namespace Bitbound.ScreenCapture.Models;

public sealed class CaptureResult : IDisposable
{
    public Bitmap? Bitmap { get; init; }

    public Exception? Exception { get; init; }
    public string FailureReason { get; init; } = string.Empty;

    [MemberNotNull(nameof(Bitmap))]
    public bool HadChanges { get; init; }

    [MemberNotNullWhen(true, nameof(Exception))]
    public bool HadException => Exception is not null;

    public bool IsSuccess { get; init; }

    public void Dispose()
    {
        Bitmap?.Dispose();
    }

    internal static CaptureResult AcquireNextFrame(HRESULT result)
    {

        if (result == HRESULT.DXGI_ERROR_WAIT_TIMEOUT)
        {
            return new CaptureResult()
            {
                FailureReason = "Timed out while waiting for the next frame.",
                IsSuccess = true
            };
        }
        return new CaptureResult()
        {
            FailureReason = "TryAcquireFrame returned failure.",
        };
    }

    internal static CaptureResult Fail(string failureReason)
    {
        return new CaptureResult()
        {
            FailureReason = failureReason
        };
    }

    internal static CaptureResult Fail(Exception exception, string? failureReason = null)
    {
        return new CaptureResult()
        {
            FailureReason = failureReason ?? exception.Message,
            Exception = exception,
        };
    }

    internal static CaptureResult NoAccumulatedFrames()
    {
        return new CaptureResult()
        {
            FailureReason = "No frames were accumulated.",
            IsSuccess = true,
        };
    }

    internal static CaptureResult Ok(Bitmap bitmap)
    {
        return new CaptureResult()
        {
            Bitmap = bitmap,
            HadChanges = true,
            IsSuccess = true,
        };
    }
}