using System.Data.Common;
using CE_API_Test.TestUtilities.Test;
using CE_API_V2.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CE_API_Test.TestUtilities;

internal class CardioExplorerServer : WebApplicationFactory<Program>
{
    private readonly string? _country;

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

            services.Configure<TestAuthHandlerOptions>(options =>
            {
                options.Country = _country ?? "CH";
                options.DefaultUserId = "Default";
        });

            services.AddAuthentication(TestAuthHandler.AuthenticationScheme)
                .AddScheme<TestAuthHandlerOptions, TestAuthHandler>(TestAuthHandler.AuthenticationScheme, options => { });
            
            services.AddControllers()
                .PartManager.ApplicationParts.Add(new AssemblyPart(typeof(TestController).Assembly));

        }); 
        builder.UseEnvironment("Development");
    }
}