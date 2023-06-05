using System.Text.Json;
using System.Text.Json.Serialization;
using CE_API_V2.Services;
using CE_API_V2.Utility;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>()
    .Build();
var allowSpecificOrigins = "AllowSpecific";

#region UOW
builder.Services.AddScoped<IBiomarkersTemplateService, BiomarkersTemplateService>();
#endregion

var allowedHosts = config.GetSection("AllowedHosts").GetChildren().Select(x => x.Value).ToArray();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins(allowedHosts);
            policy.WithHeaders("*");
            policy.WithMethods("*");
        });
});

builder.Services.AddControllers()

    .AddJsonOptions(options =>
    {

        // Use PascalCase property names during serialization

        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

    });
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<IAiRequestService, AiRequestService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7265");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cardio Explorer API v1");
});

app.UseCors(allowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();