using CE_API_V2.Data;
using CE_API_V2.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CE_API_Test.TestUtilities
{
    internal static class DBContextUtility
    {
        public static void PrepareDbContext(CardioExplorerServer app, UserModel userModel)
        {
            using var scope = app.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var dbContext = serviceProvider.GetRequiredService<CEContext>();

            if (dbContext.Users.Any())
            {
                ContextSeeder.UpdateUser(dbContext, userModel);
            }
            else
            {
                ContextSeeder.InsertUser(dbContext, userModel);
            }
        }
    }
}
