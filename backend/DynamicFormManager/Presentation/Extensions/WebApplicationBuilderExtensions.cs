using System.Text;
using Application.Services.Hasher;
using Application.Services.Jwt;
using Application.Services.Jwt.Options;
using Application.Services.SearchText;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence.Context;
using Presentation.Constants;
using Serilog;
using Serilog.Events;

namespace Presentation.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder ConfigureCqrs(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediator();
        return builder;
    }

    public static WebApplicationBuilder ConfigureControllers(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddOpenApi();
        return builder;
    }

    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ISearchTextProvider, DummySearchTextProvider>();
        return builder;
    }

    public static WebApplicationBuilder ConfigureAuthenticationAndAuthorization(this WebApplicationBuilder builder)
    {
        var section = builder.Configuration.GetRequiredSection(ConfigurationConstant.JwtOptions);
        builder.Services.Configure<JwtOptions>(section);
        var validIssuer = section["Issuer"]!;
        var validAudience = section["Audience"]!;
        var secretKey = section["SecurityKey"]!;
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = validAudience,
                    ValidIssuer = validIssuer,
                    IssuerSigningKey = key,
                    ValidAlgorithms = [SecurityAlgorithms.HmacSha256],
                    ClockSkew = TimeSpan.Zero,
                };
            });
        builder.Services.AddAuthorization();
        builder.Services.AddSingleton<IPasswordHasher, DefaultPasswordHasher>();
        builder.Services.AddSingleton<IJwtProvider, JwtProvider>();
        return builder; 
    }

    public static WebApplicationBuilder ConfigureDbContext(this WebApplicationBuilder builder)
    {
        builder.Services.AddPooledDbContextFactory<FormContext>(optionsBuilder =>
        {
            optionsBuilder.UseInMemoryDatabase(ConfigurationConstant.DbName)
                .EnableSensitiveDataLogging(builder.Environment.IsDevelopment());
        });
        return builder;
    }

    public static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder builder)
    {
        const string logMessageTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {EnvironmentName} {CorrelationId} {Level:u3}] {Username} {ClientIp} {RequestPath} {Message:lj}{NewLine}{Exception}";
        var logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithEnvironmentName()
            .Enrich.WithClientIp()
            .WriteTo
            .Async(configuration =>
            {
                configuration.Console(restrictedToMinimumLevel: LogEventLevel.Information, outputTemplate: logMessageTemplate);
                if (builder.Environment.IsDevelopment())
                {
                    configuration.File("logs.log", rollingInterval: RollingInterval.Hour, restrictedToMinimumLevel: LogEventLevel.Information, outputTemplate: logMessageTemplate);
                }
            })
            .CreateLogger();
        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(logger);
        return builder;
    }

    public static WebApplicationBuilder ConfigureCors(this WebApplicationBuilder builder, string policyName = ConfigurationConstant.AllowAllCors)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(policyName, policyBuilder =>
            {
                policyBuilder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
        return builder;
    }
}