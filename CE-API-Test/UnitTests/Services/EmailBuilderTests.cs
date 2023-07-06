using CE_API_Test.TestUtilities;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.Services;
using Moq;
using CE_API_V2.Models.Records;
using Microsoft.Extensions.Configuration;

namespace CE_API_Test.UnitTests.Services
{
    [TestFixture]
    public class EmailBuilderTests
    {
        private IEmailTemplateProvider _emailTemplateProvider;
        private IConfiguration _configuration;

        [SetUp]
        public void SetUp()
        {
            var testConfig = new Dictionary<string, string>()
            {
                { "AzureCommunicationService:Endpoint", "https://ce-v2-communication-services.communication.azure.com/" },
                { "AzureCommunicationService:MailFrom", "DoNotReply@70cd7cba-30aa-4feb-9e09-06c33c97bfb5.azurecomm.net" },
                { "AzureCommunicationService:RequestMailSubject", "CardioExplorer: new user" }
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(testConfig)
                .Build();
            var emailTemplateProvider = new Mock<IEmailTemplateProvider>();
            var htmlBodyMock = MockDataProvider.GetHtmlBodyMock();

            emailTemplateProvider.Setup(x => x.GetRequestBodyTemplate()).Returns(htmlBodyMock);

            _emailTemplateProvider = emailTemplateProvider.Object;
        }

        [Test]
        public async Task ProcessAccessRequest_GivenAccessDto_ExpectedProcessedRequest()
        {
            //Arrange
            var accessRequestDto = MockDataProvider.GetMockedAccessRequestDto();
            var sut = new EmailBuilder(_emailTemplateProvider, _configuration);
            var expectedEmailBody =
                $"New user tried to register their account: email: {accessRequestDto.EmailAddress} ({accessRequestDto.FirstName} {accessRequestDto.Surname}, {accessRequestDto.PhoneNumber})<br/><br/> With kind regards<br/> Exploris Health";

            //Act 
            var result = sut.GetEmailConfiguration(accessRequestDto);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<EMailConfiguration>();
            result.Subject.Should().Be("CardioExplorer: new user");
            result.HtmlContent.Should().Be(expectedEmailBody);
            result.Sender.Should().Be("DoNotReply@70cd7cba-30aa-4feb-9e09-06c33c97bfb5.azurecomm.net");
        }
    }
}