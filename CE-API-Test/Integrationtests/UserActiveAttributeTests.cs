using CE_API_Test.TestUtilities;
using CE_API_V2.Data;
using CE_API_V2.Models;
using System.Net;

namespace CE_API_Test.Integrationtests
{
    [TestFixture]
    public class UserActiveAttributeTests
    {

        [Test]
        [TestCase(true, HttpStatusCode.OK)]
        [TestCase(false, HttpStatusCode.Forbidden)]
        public async Task UserActiveAttribute_CallingMethodWithUserActiveAttribute_ReturnsExpectedHttpStatusCode(bool isActive, HttpStatusCode expectedStatusCode)
        {
            //Arrange
            var app = new CardioExplorerServer();
            var client = app.CreateClient();

            var user = CreateUserModel(isActive);
            PrepareDbContext(app, user);

            //Act
            var response = await client.GetAsync("/api/user");

            //Assert
            response.StatusCode.Should().Be(expectedStatusCode);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task UserActiveAttribute_CallingMethodWithAllowInActiveAttribute_ReturnsExpectedHttpStatusCode(bool isActive)
        {
            //Arrange
            var expectedStatusCode = HttpStatusCode.BadRequest;
            var app = new CardioExplorerServer();
            var client = app.CreateClient();

            var user = CreateUserModel(isActive);
            PrepareDbContext(app, user);

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
