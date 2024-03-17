using Bitbound.ScreenCapture.Extensions;
using Bitbound.ScreenCapture.Models;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Direct3D;
using Windows.Win32.Graphics.Dxgi;

namespace Bitbound.ScreenCapture.Helpers;

internal static class DxOutputGenerator
{
    private static readonly ConcurrentDictionary<string, DxOutput> _outputs = new();

    static DxOutputGenerator()
    {
        AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
    }

    public static DxOutput[] GetDxOutputs()
    {
        try
        {
            var outputs = new List<DxOutput>();

            var factoryGuid = typeof(IDXGIFactory1).GUID;
            var factoryResult = PInvoke.CreateDXGIFactory1(factoryGuid, out var factoryObj);

            var factory = (IDXGIFactory1)factoryObj;
            var adapters = factory.GetAdapters();

            foreach (var adapter in adapters)
            {
                foreach (var output in adapter.GetOutputs())
                {
                    unsafe
                    {
                        output.GetDesc(out var outputDescription);
                        var deviceName = outputDescription.DeviceName.ToString();

                        if (_outputs.TryGetValue(deviceName, out var cachedOutput) &&
                            !cachedOutput.IsDisposed)
                        {
                            outputs.Add(cachedOutput);
                            continue;
                        }

                        var bounds = outputDescription.DesktopCoordinates.ToRectangle();

                        var featureLevel = D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_11_1;
                        var featureLevelOut = &featureLevel;
                        var featureLevelArray = new[]
                        {
                            D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_11_1,
                            D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_11_0
                        };

                        fixed (D3D_FEATURE_LEVEL* featureLevelArrayRef = featureLevelArray)
                        {
                            PInvoke.D3D11CreateDevice(
                                pAdapter: adapter,
                                DriverType: 0,
                                Software: HMODULE.Null,
                                Flags: 0,
                                pFeatureLevels: featureLevelArrayRef,
                                FeatureLevels: 2,
                                SDKVersion: 7,
                                ppDevice: out var device,
                                pFeatureLevel: featureLevelOut,
                                ppImmediateContext: out var deviceContext);

                            var texture2d = DxTextureHelper.Create2dTextureDescription(bounds.Width, bounds.Height);

                            output.DuplicateOutput(device, out var outputDuplication);

                            var dxOutput = new DxOutput(
                                deviceName,
                                bounds,
                                adapter,
                                device,
                                deviceContext,
                                outputDuplication,
                                outputDescription.Rotation,
                                () => _outputs.Remove(deviceName, out _));

                            outputs.Add(dxOutput);
                            _outputs.AddOrUpdate(deviceName, dxOutput, (k, v) =>
                            {
                                v.Dispose();
                                return dxOutput;
                            });
                        }
                    }
                }
            }

            Marshal.FinalReleaseComObject(factoryObj);

            return [.. outputs];
        }
        catch
        {
            return [];
        }
    }

    private static void CurrentDomain_ProcessExit(object? sender, EventArgs e)
    {
        Disposer.TryDispose(_outputs.Values.ToArray());
    }
}
