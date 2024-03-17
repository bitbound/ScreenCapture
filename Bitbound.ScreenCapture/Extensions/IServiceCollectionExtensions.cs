using Microsoft.Extensions.DependencyInjection;

namespace Bitbound.ScreenCapture.Extensions;
public static class IServiceCollectionExtensions
{
    /// <summary>
    /// Adds <see cref="IScreenCapturer"/> as a transient service.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddScreenCapturer(this IServiceCollection services)
    {
        return services.AddTransient<IScreenCapturer, ScreenCapturer>();
    }
}
