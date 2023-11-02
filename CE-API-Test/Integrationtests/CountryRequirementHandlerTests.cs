using CE_API_Test.TestUtilities;
using CE_API_V2.Data;
using CE_API_V2.Models;
using System.Net;
using Microsoft.Extensions.DependencyInjection;

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
            DBContextUtility.PrepareDbContext(app, user);

            //Act
            var response = await client.PostAsync("/api/user", null);

            //Assert
            response.StatusCode.Should().Be(expectedStatusCode);
        }
        private UserModel CreateUserModel(bool isActive)
        {
            var user = MockDataProvider.GetMockedUserModel();
            user.UserId = "UserId";
            user.IsActive = isActive;
            user.BiomarkerOrders = null;

            return user;
        }
    }
}
