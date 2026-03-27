using ClubeBeneficios.Partners.Domain.Enums;

namespace ClubeBeneficios.Partners.Domain.Extensions;

public static class PartnerSortOptionExtensions
{
    public static string ToDatabaseValue(this PartnerSortOption sortOption)
    {
        return sortOption switch
        {
            PartnerSortOption.Name => "name",
            PartnerSortOption.Benefits => "benefits",
            PartnerSortOption.ConvertedClients => "converted_clients",
            PartnerSortOption.Campaigns => "campaigns",
            PartnerSortOption.Raffles => "raffles",
            PartnerSortOption.Performance => "performance",
            _ => "name"
        };
    }
}
