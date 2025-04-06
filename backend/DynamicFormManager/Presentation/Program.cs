using Presentation.Constants;
using Presentation.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args)
    .ConfigureLogging()
    .ConfigureValidators()
    .ConfigureCqrs()
    .ConfigureAuthenticationAndAuthorization()
    .ConfigureServices()
    .ConfigureDbContext()
    .ConfigureCors()
    .ConfigureControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseCustomExceptionHandler();
app.UseCors(ConfigurationConstant.AllowAllCors);
app.UseAuthentication();
app.UseAuthorization();
app.UseLogContextEnrichment();
app.MapControllers();
app.Run();