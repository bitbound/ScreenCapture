# ScreenCapture
A library for DirectX-accelerated screenshots and bitmap utilities.


## Quickstart (register)

These will register `IScreenCapturer` and `IBitmapUtility` as Singleton services.


### Using Dependency Injection:

```
using Bitbound.ScreenCapture.Extensions;

services.AddScreenCapturer();
```

### Using Hosting Extensions:
```
using Bitbound.ScreenCapture.Extensions;

await Host
	.CreateDefaultBuilder(args)
	.ConfigureServices((IServiceCollection services) =>
	{
		services.AddScreenCapturer();
	})
	.Build()
	.RunAsync();
```


### Without Dependency Injection:
```
using Bitbound.ScreenCapture

var capturer = ScreenCapturer.CreateDefault();
```

## Quickstart (capture)

```
var displays = capturer.GetDisplays();
var display1 = displays.First();

using var result = capturer.Capture(
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