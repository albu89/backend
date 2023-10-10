using Azure.Identity;
using CE_API_V2.Data;
using CE_API_V2.Controllers.Middlewares;
using CE_API_V2.Host;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment.EnvironmentName;

var jsonName = $"appsettings.{env}.json";

// Add services to the container.
var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile($"appsettings.json")
    .AddJsonFile(jsonName, optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>()
    .Build();

if (builder.Environment.IsProduction())
{
    var keyVaultEndpoint = config["Azure:KeyVaultEndpoint"];

    if (!string.IsNullOrEmpty(keyVaultEndpoint))
    {
        builder.Configuration.AddAzureKeyVault(new Uri(keyVaultEndpoint), new DefaultAzureCredential()).Build();
    }
}

ConfigurationUtilities.SetupCardioExplorerServices(config, builder.Services);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CEContext>();
    if (dbContext.Database.IsRelational())
    { 
        dbContext.Database.Migrate();
    }
}

// Configure the HTTP request pipeline.
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.yaml", "Cardio Explorer API v1");
        c.OAuthClientId(config["Azure:AD:ClientId"]);
        c.OAuthUsePkce();
        c.OAuthScopeSeparator(" ");
    });
    app.UseStaticFiles();
    
    app.UseReDoc(c =>
    {
        c.InjectStylesheet("redoc.css");
    });

app.MapHealthChecks("/health");

var allowedHosts = config.GetSection("AllowedHosts").GetChildren().Select(x => x.Value).ToArray();

app.UseCors(options =>
{
    options.WithOrigins(allowedHosts);
    options.AllowAnyHeader();
    options.AllowAnyMethod();
    options.AllowCredentials();
    options.WithExposedHeaders("x-api-version");
});

app.UseMiddleware<StaticInformationMiddleware>();

app.UseHttpsRedirection();

app.MapControllers();

app.UseRateLimiter();

app.Run();