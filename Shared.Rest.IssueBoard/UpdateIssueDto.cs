namespace Shared.Rest.IssueBoard;

public class UpdateIssueDto
{
    public string? Category { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IssueStatus Status { get; set; }
    public string? Resolution { get; set; }
    public string? ResolverName { get; set; }
}
