using Presentation.Constants;
using Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args)
    .ConfigureLogging()
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
}

app.UseHttpsRedirection();
app.UseCors(ConfigurationConstant.AllowAllCors);
app.UseAuthentication();
app.UseAuthorization();
app.UseLogContextEnrichment();
app.MapControllers();
app.Run();