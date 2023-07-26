using AutoMapper;
using Azure.Identity;
using CE_API_V2.Data;
using CE_API_V2.Hasher;
using CE_API_V2.Models.Mapping;
using System.Text.Json;
using System.Text.Json.Serialization;
using CE_API_V2.Models.DTO;
using CE_API_V2.Services;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.UnitOfWorks;
using CE_API_V2.UnitOfWorks.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using CE_API_V2.Utility;
using CE_API_V2.Validators;
using FluentValidation;
using CE_API_V2.Localization.JsonStringFactroy;
using Microsoft.Extensions.Localization;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>()
    .Build();
var azureAdSection = config.GetSection("Azure:AD");

if (builder.Environment.IsProduction())
{
    var keyVaultEndpoint = config["Azure:KeyVaultEndpoint"];

    if (!string.IsNullOrEmpty(keyVaultEndpoint))
    {
        builder.Configuration.AddAzureKeyVault(new Uri(keyVaultEndpoint), new DefaultAzureCredential()).Build();
    }
}

#region SQL

var connString = builder.Configuration.GetConnectionString("DefaultConnectionString");
builder.Services.AddDbContext<CEContext>(options => options.UseSqlServer(connString));

#endregion

#region Mapper

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddSingleton<UserHelper>();

#endregion

#region UOW

builder.Services.AddScoped<IBiomarkersTemplateService, BiomarkersTemplateService>();
builder.Services.AddScoped<IValidator<ScoringRequest>, ScoringRequestValidator>();
builder.Services.AddScoped<IPatientIdHashingUOW, PatientIdHashingUOW>();
builder.Services.AddScoped<IScoringUOW, ScoringUOW>();
builder.Services.AddScoped<IValueConversionUOW, ValueConversionUOW>();
builder.Services.AddScoped<IUserUOW, UserUOW>();
builder.Services.AddScoped<IInputValidationService, InputValidationService>();
builder.Services.AddScoped<IUserInformationExtractor, UserInformationExtractor>();
builder.Services.AddScoped<ICommunicationService, CommunicationService>();
builder.Services.AddScoped<IEmailTemplateProvider, EmailTemplateProvider>();
builder.Services.AddScoped<IEmailBuilder, EmailBuilder>();
builder.Services.AddScoped<IEmailClientService, EmailClientService>();
builder.Services.AddScoped<IScoringTemplateService, ScoringTemplateService>();
builder.Services.AddScoped<IScoreSummaryUtility, ScoreSummaryUtility>();

#endregion


#region Localization
builder.Services.AddLocalization();
builder.Services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
#endregion

#region Validation
builder.Services.AddSingleton(new ScoringRequestValidator());
#endregion

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(azureAdSection);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SwaggerPolicy", policy =>
    {
        policy.RequireAuthenticatedUser();
    });
    
    // options.AddPolicy("Administrator", policy =>
    // {
    //     policy.RequireRole("CE.User");
    // });
});

builder.Services.AddControllers(
        options =>
        {
            options.AllowEmptyInputInBodyModelBinding = true;
        })
    .AddJsonOptions(jsonOptions =>
    {
        jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        jsonOptions.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "OAuth2.0 Auth Code with PKCE",
        Name = "oauth2",
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri(config["AuthorizationUrl"]),
                TokenUrl = new Uri(config["TokenUrl"]),
                Scopes = new Dictionary<string, string>
                {
                    { config["ApiScope"], "read the api" }
                }
            }
        }
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
            },
            new[] { config["ApiScope"] }
        }
    });
});

builder.Services.AddHttpClient<IAiRequestService, AiRequestService>(client =>
{
    client.BaseAddress = new Uri(config.GetValue<string>("AiBaseAddress"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.yaml", "Cardio Explorer API v1");
    c.OAuthClientId(config["Azure:AD:ClientId"]);
    c.OAuthUsePkce();
    c.OAuthScopeSeparator(" ");
});

var allowedHosts = config.GetSection("AllowedHosts").GetChildren().Select(x => x.Value).ToArray();

app.UseCors(options =>
{
    options.WithOrigins(allowedHosts);
    options.AllowAnyHeader();
    options.AllowAnyMethod();
    options.AllowCredentials();
});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();