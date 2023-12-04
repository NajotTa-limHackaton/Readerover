using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Readerover.Application.Common.Identity.Services;
using Readerover.Infrastructure.Common.Caching;
using Readerover.Infrastructure.Common.Identity.Services;
using Readerover.Infrastructure.Common.Settings;
using Readerover.Persistence.Caching;
using Readerover.Persistence.DbContexts;
using Readerover.Persistence.Interceptors;
using Readerover.Persistence.Repositories;
using Readerover.Persistence.Repositories.Interfaces;
using System.Reflection;

namespace Readerover.Api.Configurations;

public static partial class HostConfiguration
{
    private static WebApplicationBuilder AddDevTools(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddSwaggerGen()
            .AddEndpointsApiExplorer();

        return builder;
    }

    private static WebApplicationBuilder AddValidators(this WebApplicationBuilder builder)
    {
        var assemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies().Select(Assembly.Load).ToList();
        assemblies.Add(Assembly.GetExecutingAssembly());

        builder.Services.AddValidatorsFromAssemblies(assemblies);

        return builder;
    }

    private static WebApplicationBuilder AddCaching(this WebApplicationBuilder builder)
    {
        builder.Services.AddLazyCache();

        builder.Services.Configure<CacheSettings>(builder.Configuration.GetSection(nameof(CacheSettings)));

        builder.Services.AddSingleton<ICacheBroker, LazyCacheBroker>();
        return builder;
    }

    private static WebApplicationBuilder AddIdentityInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IPasswordHasherService, PasswordHasherService>();

        builder.Services
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IAccountService, AccountService>();

        return builder;
    }

    private static WebApplicationBuilder AddExposers(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();

        builder.Services.AddRouting(options => options.LowercaseUrls = true);

        return builder;
    }

    private static WebApplicationBuilder AddPersistence(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<UpdateAuditableInterceptor>();

        builder.Services.AddDbContext<AppDbContext>((provider, options) =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));

            options.AddInterceptors(
                provider.GetRequiredService<UpdateAuditableInterceptor>());
        });

        return builder;
    }

    private static WebApplication UseDevTools(this WebApplication app)
    {
        app
            .UseSwagger()
            .UseSwaggerUI();

        return app;
    }

    private static WebApplication UseExposers(this WebApplication app)
    {
        app.MapControllers();

        return app;
    }
}
