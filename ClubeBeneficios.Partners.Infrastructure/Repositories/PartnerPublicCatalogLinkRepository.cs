using System.Data;
using Dapper;
using ClubeBeneficios.Partners.Domain.Dtos;
using ClubeBeneficios.Partners.Domain.Repositories;
using ClubeBeneficios.Partners.Infrastructure.Context;

namespace ClubeBeneficios.Partners.Infrastructure.Repositories;

public class PartnerPublicCatalogLinkRepository : IPartnerPublicCatalogLinkRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public PartnerPublicCatalogLinkRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<PartnerPublicCatalogLinkDto?> GetOrCreateAsync(
        Guid partnerId,
        Guid? createdByUserId,
        CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@PartnerId", partnerId);
        parameters.Add("@CreatedByUserId", createdByUserId);

        var command = new CommandDefinition(
            "dbo.usp_partner_public_catalog_link_get_or_create",
            parameters,
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);

        return await connection.QueryFirstOrDefaultAsync<PartnerPublicCatalogLinkDto>(command);
    }
}