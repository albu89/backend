using CE_API_V2.Models;
using CE_API_Test.TestUtilities;
using CE_API_V2.Services;
using Microsoft.Extensions.Configuration;

namespace CE_API_Test.UnitTests.Services
{
    [TestFixture]
    public class ResponsibilityDeterminerTests
    {
        private IResponsibilityDeterminer _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = MockServiceProvider.GenerateResponsibilityDeterminer();
        }
    
        [Test]
        [TestCase("Country", "", false, "CountryModelEMail")]
        [TestCase("", "Organization", false, "OrganizationModelEMail")]
        [TestCase("", "", false, "FallBackEmail")]
        [TestCase("Country", "Organization", true, "CountryModelEMail")] //Simulates that the user was created externally.
        [TestCase("Country", "", true, "CountryModelEMail")] //Simulates that the user was created externally.
        [TestCase("", "Organization", true, "FallBackEmail")] //Simulates that the user was created externally.
        [TestCase("", "", true, "FallBackEmail")] //Simulates that the user was created externally.
        public async Task GetEmailAddress_GivenMockedScoringRequest_ReturnsScoringResponse(string countryName, string organizationName, bool isActive, string expectedContact)
        {
            //Arrange

            //Act
            var result = _sut.GetEmailAddress(countryName, organizationName, isActive);

            //Assert
            result.Should().NotBeNull();
            result.Should().Be(expectedContact);
        }
    }
}
