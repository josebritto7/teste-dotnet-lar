namespace Lar.TesteDotNet.Application.Pagination;

public abstract class PaginatedQuery
{
    private int _limit = 100;

    public int Limit
    {
        get => _limit;
        set => _limit = value == 0 ? 100 : value;
    }

    public string? Cursor { get; set; }
}