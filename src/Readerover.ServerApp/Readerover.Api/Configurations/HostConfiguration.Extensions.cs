using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Readerover.Application.Common.Identity.Services;
using Readerover.Domain.Common.Constants;
using Readerover.Infrastructure.Common.Caching;
using Readerover.Infrastructure.Common.Identity.Services;
using Readerover.Infrastructure.Common.Settings;
using Readerover.Persistence.Caching;
using Readerover.Persistence.DbContexts;
using Readerover.Persistence.Interceptors;
using Readerover.Persistence.Repositories;
using Readerover.Persistence.Repositories.Interfaces;
using System.Reflection;
using System.Text;

namespace Readerover.Api.Configurations;

public static partial class HostConfiguration
{
    private static WebApplicationBuilder AddDevTools(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition(SwaggerConstants.SecurityDefinitionName, new OpenApiSecurityScheme
            {
                Name = SwaggerConstants.SecuritySchemeName,
                Type = SecuritySchemeType.ApiKey,
                Scheme = SwaggerConstants.SecurityScheme,
                In = ParameterLocation.Header,
                Description = SwaggerConstants.SwaggerAuthorizationDescription
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = SwaggerConstants.SecurityScheme
                        }
                    },
                Array.Empty<string>()
                }
            });
        });

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

    private static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {

        builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(nameof(JwtSettings)));
        builder.Services.Configure<FileSettings>(builder.Configuration.GetSection(nameof(FileSettings)));

        builder.Services
            .AddSingleton<IPasswordHasherService, PasswordHasherService>()
            .AddSingleton<IAccessTokenGeneratorService, AccessTokenGeneratorService>();


        builder.Services
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<ICategoryRepository, CategoryRepository>()
            .AddScoped<ISubCategoryRepository, SubCategoryRepository>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IAccountService, AccountService>()
            .AddScoped<IAuthService, AuthService>()
            .AddScoped<IFileValidatorService, FileValidatorService>()
            .AddScoped<ICategoryService, CategoryService>()
            .AddScoped<ISubCategoryService, SubCategoryService>();

        var jwtSettings = builder.Configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>()
            ?? throw new InvalidOperationException("Jwt settings is not configured.");

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = jwtSettings.ValidateIssuer,
                    ValidIssuer = jwtSettings.ValidIssuer,
                    ValidateAudience = jwtSettings.ValidateAudience,
                    ValidAudience = jwtSettings.ValidAudience,
                    ValidateLifetime = jwtSettings.ValidateLifeTime,
                    ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                };
            });

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
