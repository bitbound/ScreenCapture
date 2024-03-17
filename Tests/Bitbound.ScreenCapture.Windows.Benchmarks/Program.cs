//using BenchmarkDotNet.Configs;
//using BenchmarkDotNet.Running;
//using Bitbound.ScreenCapture.Windows.Benchmarks;
using Bitbound.ScreenCapture;
using Bitbound.ScreenCapture.Windows.Benchmarks;


//var config = new DebugInProcessConfig();
//var summary = BenchmarkRunner.Run<CaptureTests>(config);
//Console.WriteLine($"{summary}");

using var test = new CaptureTests();
test.DoCaptures();
test.DoEncoding();