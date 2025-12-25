using Shared.Rest.IssueBoard;

namespace Net10.Maui.Rest.IssueBoard.Helpers;

public static class IssueStatusHelper
{
    public static string GetDisplayName(this IssueStatus status)
    {
        return status switch
        {
            IssueStatus.NotStarted => "未着手",
            IssueStatus.InProgress => "着手中",
            IssueStatus.ResolutionFailed => "解決失敗",
            IssueStatus.CannotReproduce => "課題確認不能",
            IssueStatus.Resolved => "解決済み",
            _ => status.ToString()
        };
    }

    public static List<IssueStatusItem> GetAllStatuses()
    {
        return new List<IssueStatusItem>
        {
            new IssueStatusItem { Status = IssueStatus.NotStarted, DisplayName = "未着手" },
            new IssueStatusItem { Status = IssueStatus.InProgress, DisplayName = "着手中" },
            new IssueStatusItem { Status = IssueStatus.ResolutionFailed, DisplayName = "解決失敗" },
            new IssueStatusItem { Status = IssueStatus.CannotReproduce, DisplayName = "課題確認不能" },
            new IssueStatusItem { Status = IssueStatus.Resolved, DisplayName = "解決済み" }
        };
    }
}

public class IssueStatusItem
{
    public IssueStatus Status { get; set; }
    public string DisplayName { get; set; } = string.Empty;
}
