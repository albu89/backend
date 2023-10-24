using System.Net;
using CE_API_Test.TestUtilities;

namespace CE_API_Test.Integrationtests;

[TestFixture]
public class MiddlewareTests
{
    [Test]
    public async Task ServerShouldRequestTooManyRequestsPerMinute()
    {
        var app = new CardioExplorerServer();
        var client = app.CreateClient();

        for (var i = 0; i < 11; i++)
        {
            var response = await client.PostAsync("/api/user/request", null);
            var expectedCode = i < 10 ? HttpStatusCode.BadRequest : HttpStatusCode.TooManyRequests;
            response.StatusCode.Should().Be(expectedCode);
        }
    }
}