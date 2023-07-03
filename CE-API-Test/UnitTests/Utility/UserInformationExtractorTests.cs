using System.Security.Claims;
using CE_API_V2.Services;
using Microsoft.Identity.Web;

namespace CE_API_Test.UnitTests.Utility
{
    [TestFixture]
    public class UserInformationExtractorTests
    {
        [Test]
        public void GetUserIdInformation_GivenObject_CorrectlyExtractedIds()
        {
            //Arrange 
            var mockedUserId = "MockedUserId";
            var mockedTenantId = "MockedTenantId";

            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, mockedUserId));
            identity.AddClaim(new Claim(ClaimConstants.TenantId, mockedTenantId));

            var claimsPrincipal = new ClaimsPrincipal(identity);
            var sut = new UserInformationExtractor();

            //Act
            var result = sut.GetUserIdInformation(claimsPrincipal);

            //Assert
            result.Should().NotBeNull();
            result.UserId.Should().Be("MockedUserId");
            result.TenantId.Should().Be("MockedTenantId");
        }
    }
}
