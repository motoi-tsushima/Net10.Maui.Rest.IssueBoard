using Net10.Maui.Rest.IssueBoard.Helpers;
using Net10.Maui.Rest.IssueBoard.Services;
using Shared.Rest.IssueBoard;

namespace Net10.Maui.Rest.IssueBoard.Views;

[QueryProperty(nameof(IssueId), "IssueId")]
public partial class IssueDeletePage : ContentPage
{
    private readonly IssueService _issueService;
    private int _issueId;
    private IssueDto? _currentIssue;

    public int IssueId
    {
        get => _issueId;
        set
        {
            _issueId = value;
            LoadIssueAsync();
        }
    }

    public IssueDeletePage(IssueService issueService)
    {
        InitializeComponent();
        _issueService = issueService;
    }

    private async void LoadIssueAsync()
    {
        try
        {
            _currentIssue = await _issueService.GetIssueAsync(_issueId);
            if (_currentIssue != null)
            {
                AuthorNameLabel.Text = _currentIssue.AuthorName;
                CreatedAtLabel.Text = _currentIssue.CreatedAt.ToString("yyyy/MM/dd HH:mm:ss");
                CategoryLabel.Text = _currentIssue.Category ?? "-";
                TitleLabel.Text = _currentIssue.Title;
                DescriptionLabel.Text = _currentIssue.Description;
                StatusLabel.Text = _currentIssue.Status.GetDisplayName();
                ResolutionLabel.Text = _currentIssue.Resolution ?? "";
                ResolverNameLabel.Text = _currentIssue.ResolverName ?? "";
                ResolvedAtLabel.Text = _currentIssue.ResolvedAt?.ToString("yyyy/MM/dd HH:mm:ss") ?? "";
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("エラー", $"課題の読み込みに失敗しました: {ex.Message}", "OK");
        }
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlertAsync("確認", "本当に削除しますか？", "はい", "いいえ");
        if (!confirm)
        {
            return;
        }

        try
        {
            await _issueService.DeleteIssueAsync(_issueId);
            await DisplayAlertAsync("成功", "課題を削除しました", "OK");
            await Shell.Current.GoToAsync("../..");
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("エラー", $"課題の削除に失敗しました: {ex.Message}", "OK");
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}