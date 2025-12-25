using Api.Rest.IssueBoard.Models;
using Shared.Rest.IssueBoard;

namespace Api.Rest.IssueBoard.Mapping;

public static class IssueMapper
{
    public static IssueDto ToDto(this Issue issue)
    {
        return new IssueDto
        {
            Id = issue.Id,
            AuthorName = issue.AuthorName,
            CreatedAt = issue.CreatedAt,
            Category = issue.Category,
            Title = issue.Title,
            Description = issue.Description,
            Status = (IssueStatus)issue.Status,
            Resolution = issue.Resolution,
            ResolverName = issue.ResolverName,
            ResolvedAt = issue.ResolvedAt
        };
    }

    public static Issue ToModel(this CreateIssueDto dto)
    {
        return new Issue
        {
            AuthorName = dto.AuthorName,
            CreatedAt = DateTime.Now,
            Category = dto.Category,
            Title = dto.Title,
            Description = dto.Description,
            Status = (int)IssueStatus.NotStarted,
            Resolution = null,
            ResolverName = null,
            ResolvedAt = null
        };
    }

    public static void UpdateFromDto(this Issue issue, UpdateIssueDto dto)
    {
        issue.Category = dto.Category;
        issue.Title = dto.Title;
        issue.Description = dto.Description;
        issue.Status = (int)dto.Status;
        issue.Resolution = dto.Resolution;
        issue.ResolverName = dto.ResolverName;
        issue.ResolvedAt = DateTime.Now;
    }
}
