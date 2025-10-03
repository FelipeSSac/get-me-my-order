namespace Order.Application.DTO;

public class PaginatedResult<T>
{
    public IEnumerable<T> Items { get; }
    public int Page { get; }
    public int PageSize { get; }
    public int TotalItems { get; }
    public int TotalPages { get; }

    public PaginatedResult(IEnumerable<T> items, int page, int pageSize, int totalItems)
    {
        Items = items;
        Page = page;
        PageSize = pageSize;
        TotalItems = totalItems;
        TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
    }

    public PaginatedResult<TResponse> ToResponse<TResponse>(Func<T, TResponse> selector)
    {
        return new PaginatedResult<TResponse>(
            Items.Select(selector),
            Page,
            PageSize,
            TotalItems
        );
    }
}