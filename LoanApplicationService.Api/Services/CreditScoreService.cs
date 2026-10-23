using System.Net;
using System.Net.Http.Json;
using LoanApplicationService.Api.Dtos;

namespace LoanApplicationService.Api.Services;

public class CreditScoreService : ICreditScoreService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public CreditScoreService(
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<int> GetCreditScoreAsync(
        string socialSecurityNumber)
    {
        string? baseUrl =
            _configuration["CreditScoreApi:BaseUrl"];

        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            throw new InvalidOperationException(
                "CreditScoreApi:BaseUrl är inte konfigurerad.");
        }

        string encodedSocialSecurityNumber =
            Uri.EscapeDataString(socialSecurityNumber);

        string url =
            $"{baseUrl.TrimEnd('/')}/api/customers/{encodedSocialSecurityNumber}/credit-score";

        HttpResponseMessage response =
            await _httpClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            throw new KeyNotFoundException(
                $"Kunden med personnummer {socialSecurityNumber} finns inte i CreditScore-tjänsten.");
        }

        response.EnsureSuccessStatusCode();

        CreditScoreResponseDto? creditScoreResponse =
            await response.Content
                .ReadFromJsonAsync<CreditScoreResponseDto>();

        if (creditScoreResponse is null)
        {
            throw new InvalidOperationException(
                "CreditScore API returnerade inget giltigt svar.");
        }

        return creditScoreResponse.CreditScore;
    }
}