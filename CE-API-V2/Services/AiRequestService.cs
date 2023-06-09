using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.Services.Mocks;
using CE_API_V2.Utility;

namespace CE_API_V2.Services;

public class AiRequestService : IAiRequestService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly IWebHostEnvironment _env;

    public AiRequestService(HttpClient httpClient, IConfiguration config, IWebHostEnvironment env)
    {
        _httpClient = httpClient;
        _config = config;
        _env = env;
    }

    public async Task<ScoringResponse>? RequestScore(ScoringRequest scoringRequest)
    {
        if (scoringRequest is null)
            return null;

        var patientDataToAiDto = DtoConverter.ConvertToAiDto(scoringRequest.Biomarkers);
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
            if (_env.IsStaging() || _env.IsDevelopment())
            {
                ScoringResponseMocker responseMocker = new();
                scoringResponse = responseMocker.MockScoringResponse();
            }
        }

        return scoringResponse;
    }
}