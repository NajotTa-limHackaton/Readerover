namespace Readerover.Api.Configurations;

public static partial class HostConfiguration
{
    /// <summary>
    /// DI container configuration
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static ValueTask<WebApplicationBuilder> ConfigureAsync(this WebApplicationBuilder builder)
    {
        builder
            .AddDevTools()
            .AddInfrastructure()
            .AddCaching()
            .AddPersistence()
            .AddValidators()
            .AddExposers();

        return new(builder);
    }


    /// <summary>
    /// Middleware configuration
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static ValueTask<WebApplication> ConfigureAsync(this WebApplication app)
    {
        app
            .UseDevTools()
            .UseExposers();

        return new(app);
    }
}
