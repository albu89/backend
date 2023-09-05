using System.Net;
using CE_API_V2.Services;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.UnitOfWorks.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
namespace CE_API_Test.TestUtilities;

public class MockServiceProvider
{
    public static AiRequestService GenerateAiRequestService()
    {

        var inMemSettings = new Dictionary<string, string>
        {
            { "AiSubpath", "/api/AiMock?" }
        };

        var config = new ConfigurationBuilder().AddInMemoryCollection(inMemSettings!).Build();

        var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        var content = MockDataProvider.GetMockedSerializedResponse();
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(content),
        };

        httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        var httpClient = new HttpClient(httpMessageHandlerMock.Object);
        httpClient.BaseAddress = new Uri("https://testproject");

        var mockEnv = new Mock<IWebHostEnvironment>();
        mockEnv.Setup(m => m.EnvironmentName)
            .Returns("Hosting:Staging");
        return new AiRequestService(httpClient, config, mockEnv.Object);
    }

    public static ResponsibilityDeterminer GenerateResponsibilityDeterminer()
    {
        var testConfig = new Dictionary<string, string?>()
        {
            { "ExplorisContactEMail", "FallBackEmail" },
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(testConfig)
            .Build();

        var organizationModel = MockDataProvider.GetMockedOrganizationModel();
        var countryModel = MockDataProvider.GetMockedCountryModel();

        organizationModel.ContactEmail = "OrganizationModelEMail";
        countryModel.ContactEmail = "CountryModelEMail";

        var administrativeEntitiesUow = new Mock<IAdministrativeEntitiesUOW>();
        administrativeEntitiesUow.Setup(x => x.GetOrganizationByName(It.IsAny<string>()))
            .Returns(organizationModel);
        administrativeEntitiesUow.Setup(x => x.GetCountryByName(It.IsAny<string>()))
            .Returns(countryModel);

        var emailValidator= new Mock<IEmailValidator>();
        emailValidator.Setup(x=>x.EmailAddressIsValid(It.IsAny<string>())).Returns(true);

        return new ResponsibilityDeterminer(administrativeEntitiesUow.Object, configuration, emailValidator.Object);
    }
}