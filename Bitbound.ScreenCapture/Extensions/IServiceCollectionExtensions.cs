using Microsoft.Extensions.DependencyInjection;

namespace Bitbound.ScreenCapture.Extensions;
public static class IServiceCollectionExtensions
{
    /// <summary>
    /// Adds the following services with the specified lifetimes:
    /// <list type="bullet">
    /// <item>
    ///   <see cref="IScreenCapturer"/> as Transient
    /// </item>
    /// <item>
    ///   <see cref="IBitmapUtility"/> as Singleton
    /// </item>
    /// </list>
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddScreenCapturer(this IServiceCollection services)
    {
        return services
            .AddSingleton<IBitmapUtility, BitmapUtility>()
            .AddTransient<IScreenCapturer, ScreenCapturer>();
    }
}
