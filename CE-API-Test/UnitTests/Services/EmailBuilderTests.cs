using CE_API_Test.TestUtilities;
using CE_API_V2.Models.Records;
using CE_API_V2.Services;
using CE_API_V2.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Moq;

namespace CE_API_Test.UnitTests.Services
{
    [TestFixture]
    public class EmailBuilderTests
    {

        private const string ExpectedSubject = "CardioExplorer: new user";
        private const string ExpectedSender = "DoNotReply@70cd7cba-30aa-4feb-9e09-06c33c97bfb5.azurecomm.net";

        private IEmailTemplateProvider _emailTemplateProvider;
        private IConfiguration _configuration;
        private IResponsibilityDeterminer _responsibilityDeterminer;

        [SetUp]
        public void SetUp()
        {
            var testConfig = new Dictionary<string, string?>()
            {
                { "AzureCommunicationService:Endpoint", "https://ce-v2-communication-services.communication.azure.com/" },
                { "AzureCommunicationService:MailFrom", "DoNotReply@70cd7cba-30aa-4feb-9e09-06c33c97bfb5.azurecomm.net" },
                { "AzureCommunicationService:RequestMailSubject", "CardioExplorer: new user" },
                { "AzureCommunicationService:ActivateUserMailSubject", "CardioExplorer: new user" }
            };

            _configuration = GetConfiguration(testConfig);

            var emailTemplateProvider = new Mock<IEmailTemplateProvider>();
            var htmlRequestBodyMock = MockDataProvider.GetRequestHtmlBodyMock();
            var htmlActivateUserBodyMock = MockDataProvider.GetActivateUserHtmlBodyMock();
            emailTemplateProvider.Setup(x => x.GetRequestBodyTemplate()).Returns(htmlRequestBodyMock);
            emailTemplateProvider.Setup(x => x.GetActivateUserBodyTemplate()).Returns(htmlActivateUserBodyMock);
            _emailTemplateProvider = emailTemplateProvider.Object;

            var responsibilityDeterminer = new Mock<IResponsibilityDeterminer>();
            _responsibilityDeterminer = responsibilityDeterminer.Object;
        }

        [Test]
        public void GetRequestAccessEmailConfiguration_GivenAccessDto_ExpectedProcessedRequest()
        {
            //Arrange
            var accessRequestDto = MockDataProvider.GetMockedAccessRequestDto();
            var sut = new EmailBuilder(_emailTemplateProvider, _configuration, _responsibilityDeterminer);

            var expectedEmailBody = 
                $"New user tried to register their account: email: {accessRequestDto.EmailAddress} ({accessRequestDto.FirstName} {accessRequestDto.Surname}, {accessRequestDto.PhoneNumber})<br/><br/> With kind regards<br/> Exploris Health";
            var expectedSubject = "CardioExplorer: new user";
            var expectedSender = "DoNotReply@70cd7cba-30aa-4feb-9e09-06c33c97bfb5.azurecomm.net";

            //Act 
            var result = sut.GetRequestAccessEmailConfiguration(accessRequestDto);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<EMailConfiguration>();
            result.Subject.Should().Be(ExpectedSubject);
            result.HtmlContent.Should().Be(expectedEmailBody);
            result.Sender.Should().Be(ExpectedSender);
        }

        [Test]
        public void GetRequestAccessEmailConfiguration_GivenEmptyConfiguration_ExpectedProcessedRequest() 
        {
            //Arrange
            var emptyConfig = GetConfiguration(new Dictionary<string, string?>());

            var accessRequestDto = MockDataProvider.GetMockedAccessRequestDto();
            var sut = new EmailBuilder(_emailTemplateProvider, emptyConfig, _responsibilityDeterminer);

            //Act 
            var result = sut.GetRequestAccessEmailConfiguration(accessRequestDto);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<EMailConfiguration>();
        }

        [Test]
        public void GetActivateUserEmailConfiguration_GivenAccessDto_ExpectedProcessedRequest()
        {
            //Arrange
            var userModel = MockDataProvider.GetMockedUserModel();
            var sut = new EmailBuilder(_emailTemplateProvider, _configuration, _responsibilityDeterminer);

            var expectedEmailBody =
                $"New user has registered their account: email: {userModel.UserId} ({userModel.FirstName} {userModel.Surname})<br/><br/> With kind regards<br/> Exploris Health";
            var expectedSender = "DoNotReply@70cd7cba-30aa-4feb-9e09-06c33c97bfb5.azurecomm.net";
            var expectedSubject = "CardioExplorer: new user";

            //Act 
            var result = sut.GetActivateUserEmailConfiguration(userModel);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<EMailConfiguration>();
            result.Subject.Should().Be(expectedSubject);
            result.HtmlContent.Should().Be(expectedEmailBody);
            result.Sender.Should().Be(expectedSender);
        }

        [Test]
        public void GetActivateUserEmailConfiguration_GivenEmptyConfiguration_ExpectedProcessedRequest()
        {
            //Arrange
            var emptyConfig = GetConfiguration(new Dictionary<string, string?>());
            var userModel = MockDataProvider.GetMockedUserModel();
            var sut = new EmailBuilder(_emailTemplateProvider, emptyConfig, _responsibilityDeterminer);

            //Act 
            var result = sut.GetActivateUserEmailConfiguration(userModel);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<EMailConfiguration>();
        }

        private IConfiguration GetConfiguration(Dictionary<string, string?> config) => new ConfigurationBuilder()
                .AddInMemoryCollection(config)
                .Build();
    }
}