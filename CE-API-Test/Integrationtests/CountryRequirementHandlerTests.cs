using CE_API_Test.TestUtilities;
using CE_API_V2.Data;
using CE_API_V2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CE_API_Test.Integrationtests
{
    [TestFixture]
    internal class CountryRequirementHandlerTests
    {
        [Test]
        [TestCase("DE", HttpStatusCode.Forbidden)]
        [TestCase("CH", HttpStatusCode.BadRequest)]
        public async Task CountryRequirementHandler_GivenCountryCodes_ExpectedAccordingStatusCode(string countryCode, HttpStatusCode expectedStatusCode)
        {
            //Arrange
            bool isActive = true;
            var app = new CardioExplorerServer(countryCode);
            var client = app.CreateClient();

            var user = CreateUserModel(isActive);
            PrepareDbContext(app, user);

            //Act
            var response = await client.PostAsync("/api/user", null);

            //Assert
            response.StatusCode.Should().Be(expectedStatusCode);
        }

        private void PrepareDbContext(CardioExplorerServer app, UserModel userModel)
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
