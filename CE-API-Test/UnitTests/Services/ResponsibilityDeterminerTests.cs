using CE_API_Test.TestUtilities;
using CE_API_V2.Services;

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
        [TestCase("Country", "Organization", false, "OrganizationModelEMail")]
        [TestCase("Country", "Organization", true, "CountryModelEMail")] //Simulates that the user was created externally.
        public void GetEmailAddress_GivenMockedScoringRequest_ReturnsScoringResponse(string countryName, string organizationName, bool isActive, string expectedContact)
        {
            //Arrange

            //Act
            var result = _sut.GetEmailAddress(countryName, organizationName, isActive);

            //Assert
            result.Should().NotBeNull();
            result.Should().Be(expectedContact);
        }

        [Test]
        [TestCase("Country", "", false, "CountryModelEMail")]
        [TestCase("Country", "", true, "CountryModelEMail")] //Simulates that the user was created externally.
        [TestCase("", "Organization", false, "OrganizationModelEMail")]
        [TestCase("", "Organization", true, "FallBackEmail")] //Simulates that the user was created externally.
        public void GetEmailAddress_GivenMockedScoringRequestWithEmptyValues_ReturnsScoringResponse(string countryName, string organizationName, bool isActive, string expectedContact)
        {
            //Arrange

            //Act
            var result = _sut.GetEmailAddress(countryName, organizationName, isActive);

            //Assert
            result.Should().NotBeNull();
            result.Should().Be(expectedContact);
        }

        [Test]
        [TestCase("", "", false, "FallBackEmail")]
        [TestCase("", "", true, "FallBackEmail")] //Simulates that the user was created externally.
        public void GetEmailAddress_GivenMockedScoringRequestWithMissingOrganizationAndCountry_ReturnsScoringResponse(
                string countryName, string organizationName, bool isActive, string expectedContact)
        {
            //Arrange

            //Act
            var result = _sut.GetEmailAddress(countryName, organizationName, isActive);

            //Assert
            result.Should().NotBeNull();
            result.Should().Be(expectedContact);
        }

        [Test]
        public void GetEmailAddress_ResponsibilityDeterminerLacksConfiguration_ReturnsScoringResponse()
        {
            //Arrange
            var countryName = string.Empty;
            var organizationName = string.Empty;
            var isActive = true;

            var sut = MockServiceProvider.GenerateResponsibilityDeterminerWithInvalidConfiguration();

            //Act
            var result = sut.GetEmailAddress(countryName, organizationName, isActive);

            //Assert
            result.Should().NotBeNull();
            result.Should().Be(string.Empty);
        }
    }
}
