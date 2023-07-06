using Azure.Communication.Email;
using CE_API_V2.Services;
using Microsoft.Extensions.Configuration;
using Moq;

namespace CE_API_Test.UnitTests.Services
{
    [TestFixture]
    public class EmailClientServiceTests
    {
        private IConfiguration _configuration;

        [SetUp]
        public void SetUp()
        {
            var configuration = new Mock<IConfiguration>();
            var mockedSection = new Mock<IConfigurationSection>();

            mockedSection.Setup(x => x["KeyCredential"]).Returns("val1");
            mockedSection.Setup(x => x["Endpoint"]).Returns("https://localhost");
            configuration.Setup(x => x.GetSection("AzureCommunicationService")).Returns(mockedSection.Object);

            _configuration = configuration.Object;
        }

        [Test]
        public void GetEmailClient_GivenValidParameters_ExpectedValidEMailClient()
        {
            //Arrange
            var sut = new EmailClientService(_configuration);

            //Act
            var result = sut.GetEmailClient();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<EmailClient>();
        }
    }
}