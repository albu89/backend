using System.Net;
using CE_API_V2.Services;
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
}