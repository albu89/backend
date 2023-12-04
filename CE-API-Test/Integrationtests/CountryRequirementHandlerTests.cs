using CE_API_Test.TestUtilities;
using CE_API_V2.Models;
using System.Net;

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
            var app = new CardioExplorerServer(countryCode);
            var client = app.CreateClient();

            var user = CreateUserModel();
            DBContextUtility.PrepareDbContext(app, user);

            //Act
            var response = await client.PostAsync("/api/user", null);

            //Assert
            response.StatusCode.Should().Be(expectedStatusCode);
        }

        private UserModel CreateUserModel()
        {
            var user = MockDataProvider.GetMockedUserModel();
            user.UserId = "UserId";
            user.IsActive = true;
            user.BiomarkerOrders = null;

            return user;
        }
    }
}
