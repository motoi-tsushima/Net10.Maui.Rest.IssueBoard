using Net10.Maui.Rest.IssueBoard.Helpers;
using Net10.Maui.Rest.IssueBoard.Services;
using Shared.Rest.IssueBoard;

namespace Net10.Maui.Rest.IssueBoard.Views;

public partial class IssueListPage : ContentPage
{
    private readonly IssueService _issueService;

    public IssueListPage(IssueService issueService)
    {
        InitializeComponent();
        _issueService = issueService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadIssuesAsync();
    }

    private async Task LoadIssuesAsync()
    {
        try
        {
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            var issues = await _issueService.GetAllIssuesAsync();
            var displayIssues = issues.Select(i => new IssueDisplayModel
            {
                Id = i.Id,
                AuthorName = i.AuthorName,
                CreatedAt = i.CreatedAt,
                Category = i.Category ?? "",
                Title = i.Title,
                StatusDisplay = i.Status.GetDisplayName(),
                ResolverName = i.ResolverName ?? ""
            }).ToList();

            IssuesCollectionView.ItemsSource = displayIssues;
        }
        catch (Exception ex)
        {
            // 詳細なエラー情報を表示
            var innerMessage = ex.InnerException?.Message ?? "なし";
            var fullMessage = $"エラー: {ex.Message}\n内部例外: {innerMessage}\n\nAPIサーバーに接続できない可能性があります。";
            
            await this.DisplayAlert("エラー", fullMessage, "OK");
        }
        finally
        {
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
        }
    }

    private async void OnRefreshClicked(object sender, EventArgs e)
    {
        await LoadIssuesAsync();
    }

    private async void OnRefreshViewRefreshing(object sender, EventArgs e)
    {
        await LoadIssuesAsync();
        RefreshView.IsRefreshing = false;
    }

    private async void OnIssueSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is IssueDisplayModel selectedIssue)
        {
            await Shell.Current.GoToAsync($"{nameof(IssueDetailPage)}?IssueId={selectedIssue.Id}");
            IssuesCollectionView.SelectedItem = null;
        }
    }

    private async void OnCreateClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(IssueCreatePage));
    }

    private async void OnExitClicked(object sender, EventArgs e)
    {
        bool confirm = await this.DisplayAlert("確認", "アプリケーションを終了しますか?", "はい", "いいえ");
        if (confirm)
        {
            Application.Current?.Quit();
        }
    }
}

public class IssueDisplayModel
{
    public int Id { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string StatusDisplay { get; set; } = string.Empty;
    public string ResolverName { get; set; } = string.Empty;
}