namespace ClubeBeneficios.Partners.Domain.Dtos;

public class PagedResultDto<T>
{
    public IReadOnlyCollection<T> Items { get; set; } = Array.Empty<T>();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }

    public static PagedResultDto<T> Create(
        IReadOnlyCollection<T> items,
        int totalCount,
        int page,
        int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 20;

        var totalPages = totalCount == 0
            ? 0
            : (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PagedResultDto<T>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages,
            HasNextPage = totalPages > 0 && page < totalPages,
            HasPreviousPage = page > 1
        };
    }
}