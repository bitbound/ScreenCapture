# ScreenCapture
A library for DirectX-accelerated screenshots and bitmap utilities.


## Quickstart (register)

### Using Dependency Injection:

```
using Bitbound.ScreenCapture.Extensions;

services.AddScreenCapturer();

```

### Using Hosting Extensions:
```
await Host
	.CreateDefaultBuilder(args)
	.ConfigureServices((IServiceCollection services) =>
	{
		services.AddScreenCapturer();
	})
	.Build()
	.RunAsync();
```

These will register `IScreenCapturer` as a transient service and `IBitmapUtility` as a singleton.

`IScreenCapturer` should be disposed after use.


### Without Dependency Injection:
```
using var capturer = ScreenCapturer.CreateDefault();
```

## Quickstart (capture)

```
var displays = capturer.GetDisplays();
var display1 = _displays.First();

using var result = _capturer.Capture(
    targetDisplay: display1,
    captureCursor: true);

if (result.IsSuccess)
{
    // Do something with result.Bitmap.
}
else
{
    // Do something with result failure properties.
}
```