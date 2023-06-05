using CE_API_V2.DTO;
using CE_API_V2.Models;
using CE_API_V2.Service.Mock;
using CE_API_V2.Utility;

namespace CE_API_V2.Services;

public class AiRequestService : IAiRequestService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public AiRequestService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<ScoringResponse>? RequestScore(ScoringRequestDto scoringRequestDto)
    {
        if (scoringRequestDto is null)
            return null;

        var patientDataToAiDto = DataTransferUtility.ConvertBiomarkersToAiDto(scoringRequestDto);
        var response = await GetScoreAsync(patientDataToAiDto);

        return response;
    }

    private async Task<ScoringResponse> GetScoreAsync(AiDto patientDataDto)
    {
        var requestString = $"{_config.GetValue<string>("AiSubpath")}{DataTransferUtility.CreateQueryString(patientDataDto)}";
        var fullstring = _httpClient.BaseAddress + requestString;
        ScoringResponse scoringResponse = null;
        HttpResponseMessage response = null;

        try
        {
            response = await _httpClient.GetAsync(requestString);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            scoringResponse = DataTransferUtility.FormatResponse(jsonResponse);
        }
        catch (HttpRequestException e) //Todo
        {
#if DEBUG
            ScoringResponseMocker responseMocker = new();
            scoringResponse = responseMocker.MockScoringResponse();
#endif
        }

        return scoringResponse;
    }
}