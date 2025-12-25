using Net10.Maui.Rest.IssueBoard.Helpers;
using Net10.Maui.Rest.IssueBoard.Services;
using Shared.Rest.IssueBoard;

namespace Net10.Maui.Rest.IssueBoard.Views;

[QueryProperty(nameof(IssueId), "IssueId")]
public partial class IssueDetailPage : ContentPage
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

    public IssueDetailPage(IssueService issueService)
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
            await DisplayAlertAsync("ÉGÉâÅ[", $"â€ëËÇÃì«Ç›çûÇ›Ç…é∏îsÇµÇ‹ÇµÇΩ: {ex.Message}", "OK");
        }
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(IssueEditPage)}?IssueId={_issueId}");
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(IssueDeletePage)}?IssueId={_issueId}");
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}