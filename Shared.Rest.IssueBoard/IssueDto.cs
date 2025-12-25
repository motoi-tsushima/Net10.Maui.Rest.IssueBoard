namespace Shared.Rest.IssueBoard;

public class IssueDto
{
    public int Id { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string? Category { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IssueStatus Status { get; set; }
    public string? Resolution { get; set; }
    public string? ResolverName { get; set; }
    public DateTime? ResolvedAt { get; set; }
}
