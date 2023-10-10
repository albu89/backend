using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using AutoMapper;
using CE_API_V2.Data;
using CE_API_V2.Hasher;
using CE_API_V2.Localization.JsonStringFactroy;
using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Mapping;
using CE_API_V2.Services;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.UnitOfWorks;
using CE_API_V2.UnitOfWorks.Interfaces;
using CE_API_V2.Utility;
using CE_API_V2.Utility.Auth;
using CE_API_V2.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
namespace CE_API_V2.Host;

public static class ConfigurationUtilities
{
    public static void SetupCardioExplorerServices(IConfigurationRoot configurationRoot, IServiceCollection services)
    {

#region SQL

        var azureAdSection = configurationRoot.GetSection("Azure:AD");
        var connString = configurationRoot.GetConnectionString("DefaultConnectionString");

        services.AddDbContext<CEContext>(options => options.UseSqlServer(connString));

#endregion

#region Mapper

        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingProfile());
        });

        IMapper mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);

        services.AddSingleton<UserHelper>();

#endregion

#region UOW

        services.AddScoped<IBiomarkersTemplateService, BiomarkersTemplateService>();
        services.AddScoped<IValidator<ScoringRequest>, ScoringRequestValidator>();
        services.AddScoped<IPatientIdHashingUOW, PatientIdHashingUOW>();
        services.AddScoped<IScoringUOW, ScoringUOW>();
        services.AddScoped<IValueConversionUOW, ValueConversionUOW>();
        services.AddScoped<IUserUOW, UserUOW>();
        services.AddScoped<IInputValidationService, InputValidationService>();
        services.AddScoped<IUserInformationExtractor, UserInformationExtractor>();
        services.AddScoped<ICommunicationService, CommunicationService>();
        services.AddScoped<IEmailTemplateProvider, EmailTemplateProvider>();
        services.AddScoped<IEmailBuilder, EmailBuilder>();
        services.AddScoped<IEmailClientService, EmailClientService>();
        services.AddScoped<IScoringTemplateService, ScoringTemplateService>();
        services.AddScoped<IScoreSummaryUtility, ScoreSummaryUtility>();
        services.AddScoped<IResponsibilityDeterminer, ResponsibilityDeterminer>();
        services.AddScoped<IAdministrativeEntitiesUOW, AdministrativeEntitiesUOW>();
        services.AddScoped<IEmailValidator, EmailValidator>(); 

        #endregion

        #region Localization

        services.AddLocalization();
        services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();

#endregion

#region Validation

        services.AddSingleton<ScoringRequestValidator>();

#endregion

        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(azureAdSection);

        var countryCode = configurationRoot.GetValue<string>("Country") ?? "CH";

        services.AddSingleton<IAuthorizationHandler, CountryRequirementHandler>();

        var countryPolicy = new AuthorizationPolicyBuilder()
            .AddRequirements(new CountryRequirement(countryCode)).Build();
        ;

        services.AddAuthorization(options =>
        {
            options.DefaultPolicy = countryPolicy;
            options.AddPolicy("SwaggerPolicy", policy =>
            {
                policy.RequireAuthenticatedUser();
            });
            options.AddPolicy("Administrator", policy =>
            {
                policy.RequireRole("CE.Admin", "CE.SystemAdmin");
            });

            options.AddPolicy("CountryPolicy", countryPolicy);
        });

        services.AddControllers(
                options =>
                {
                    options.AllowEmptyInputInBodyModelBinding = true;
                })
            .AddJsonOptions(jsonOptions =>
            {
                jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                jsonOptions.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        services.AddHealthChecks();

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c =>
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
                        AuthorizationUrl = new Uri(configurationRoot["AuthorizationUrl"]),
                        TokenUrl = new Uri(configurationRoot["TokenUrl"]),
                        Scopes = new Dictionary<string, string>
                        {
                            { configurationRoot["ApiScope"], "read the api" }
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
                    new[] { configurationRoot["ApiScope"] }
                }
            });
            var filePath = Path.Combine(System.AppContext.BaseDirectory, "CE-API-V2.xml");
            c.IncludeXmlComments(filePath);
            c.EnableAnnotations();
        });

        services.AddHttpClient<IAiRequestService, AiRequestService>(client =>
        {
            client.BaseAddress = new Uri(configurationRoot.GetValue<string>("AiBaseAddress"));
        });

        services.AddRateLimiter(options =>
        {
            options.AddPolicy("RequestLimitPerMinute", context => RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: context.Connection.RemoteIpAddress,
                factory: _ => new FixedWindowRateLimiterOptions()
                {
                    AutoReplenishment = false,
                    PermitLimit = 10,
                    QueueLimit = 0,
                    Window = TimeSpan.FromMinutes(1)
                }));
            

            options.OnRejected = async (context, token) =>
            {
                context.HttpContext.Response.StatusCode = 429;
                await context.HttpContext.Response.WriteAsync("Too many requests.", cancellationToken: token);
            };
        });
    }
}