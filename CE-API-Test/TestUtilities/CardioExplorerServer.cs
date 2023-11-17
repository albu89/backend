using System.Data.Common;
using CE_API_Test.TestUtilities.Test;
using CE_API_V2.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CE_API_Test.TestUtilities;

internal class CardioExplorerServer : WebApplicationFactory<Program>
{
    private readonly string? _country;
    public string DefaultUserId { get; set; } = "1";
    public string Environment { get; set; } = Constants.DevelopmentEnvironment;

    public CardioExplorerServer()
    {
    }

    public CardioExplorerServer(string country)
    {
        _country = country;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<CEContext>));

            services.Remove(dbContextDescriptor);

            var dbConnectionDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbConnection));

            services.Remove(dbConnectionDescriptor);

            services.AddDbContext<CEContext>((container, options) =>
            {
                options.UseInMemoryDatabase("IntegrationTestDB");
            });

            if (Environment == "Testing")
            {
                services.Configure<TestAuthHandlerOptions>(options => options.DefaultUserId = DefaultUserId);
            }
            services.AddAuthentication(TestAuthHandler.AuthenticationScheme)
                    .AddScheme<TestAuthHandlerOptions, TestAuthHandler>(TestAuthHandler.AuthenticationScheme, options => { });

            services.Configure<TestAuthHandlerOptions>(options =>
            {
                options.Country = _country ?? "CH";
            });
        }); 
        builder.UseEnvironment(Environment);
    }
}