using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;

namespace ClubeBeneficios.Partners.Infrastructure.Clients.Identity;

public class IdentityPartnerInvitationClient : IIdentityPartnerInvitationClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public IdentityPartnerInvitationClient(
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<PartnerInvitationCreatedResponse?> CreateInvitationAsync(
        Guid partnerId,
        Guid? createdByUserId,
        CancellationToken cancellationToken = default)
    {
        var internalApiKey = _configuration["IdentityApi:InternalApiKey"];
        var activationBaseUrl = _configuration["IdentityApi:ActivationBaseUrl"];
        var expirationDays = _configuration.GetValue<int?>("IdentityApi:InvitationExpirationDays") ?? 7;

        if (string.IsNullOrWhiteSpace(internalApiKey))
        {
            throw new InvalidOperationException("IdentityApi:InternalApiKey não configurada.");
        }

        var request = new CreateInternalPartnerInvitationRequest
        {
            PartnerId = partnerId,
            CreatedByUserId = createdByUserId,
            ExpirationDays = expirationDays,
            ActivationBaseUrl = activationBaseUrl
        };

        using var httpRequest = new HttpRequestMessage(
            HttpMethod.Post,
            "api/internal/partner-invitations")
        {
            Content = JsonContent.Create(request)
        };

        httpRequest.Headers.Add("X-Internal-Api-Key", internalApiKey);

        using var response = await _httpClient.SendAsync(httpRequest, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);

            throw new InvalidOperationException(
                $"Falha ao criar convite de acesso na Identity API. StatusCode: {(int)response.StatusCode}. Body: {body}");
        }

        return await response.Content.ReadFromJsonAsync<PartnerInvitationCreatedResponse>(
            cancellationToken: cancellationToken);
    }
}