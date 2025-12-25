using Net10.Maui.Rest.IssueBoard.Helpers;
using Net10.Maui.Rest.IssueBoard.Services;
using Shared.Rest.IssueBoard;

namespace Net10.Maui.Rest.IssueBoard.Views;

[QueryProperty(nameof(IssueId), "IssueId")]
public partial class IssueEditPage : ContentPage
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

    public IssueEditPage(IssueService issueService)
    {
        InitializeComponent();
        _issueService = issueService;
        InitializeStatusPicker();
    }

    private void InitializeStatusPicker()
    {
        var statuses = IssueStatusHelper.GetAllStatuses();
        StatusPicker.ItemsSource = statuses;
        StatusPicker.ItemDisplayBinding = new Binding("DisplayName");
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
                CategoryEntry.Text = _currentIssue.Category;
                TitleEntry.Text = _currentIssue.Title;
                DescriptionEditor.Text = _currentIssue.Description;
                ResolutionEditor.Text = _currentIssue.Resolution;
                ResolverNameEntry.Text = _currentIssue.ResolverName;

                var statuses = IssueStatusHelper.GetAllStatuses();
                var selectedStatus = statuses.FirstOrDefault(s => s.Status == _currentIssue.Status);
                StatusPicker.SelectedItem = selectedStatus;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("エラー", $"課題の読み込みに失敗しました: {ex.Message}", "OK");
        }
    }

    private async void OnUpdateClicked(object sender, EventArgs e)
    {
        if (!ValidateInput())
        {
            return;
        }

        try
        {
            var selectedStatus = (IssueStatusItem)StatusPicker.SelectedItem;
            var updateDto = new UpdateIssueDto
            {
                Category = string.IsNullOrWhiteSpace(CategoryEntry.Text) ? null : CategoryEntry.Text,
                Title = TitleEntry.Text,
                Description = DescriptionEditor.Text,
                Status = selectedStatus.Status,
                Resolution = string.IsNullOrWhiteSpace(ResolutionEditor.Text) ? null : ResolutionEditor.Text,
                ResolverName = string.IsNullOrWhiteSpace(ResolverNameEntry.Text) ? null : ResolverNameEntry.Text
            };

            await _issueService.UpdateIssueAsync(_issueId, updateDto);
            await DisplayAlertAsync("成功", "課題を更新しました", "OK");
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("エラー", $"課題の更新に失敗しました: {ex.Message}", "OK");
        }
    }

    private bool ValidateInput()
    {
        bool isValid = true;

        TitleErrorLabel.IsVisible = false;
        DescriptionErrorLabel.IsVisible = false;

        if (string.IsNullOrWhiteSpace(TitleEntry.Text))
        {
            TitleErrorLabel.Text = "課題タイトルは必須です";
            TitleErrorLabel.IsVisible = true;
            isValid = false;
        }
        else if (TitleEntry.Text.Length > 100)
        {
            TitleErrorLabel.Text = "課題タイトルは100文字以内で入力してください";
            TitleErrorLabel.IsVisible = true;
            isValid = false;
        }

        if (string.IsNullOrWhiteSpace(DescriptionEditor.Text))
        {
            DescriptionErrorLabel.Text = "課題の文面は必須です";
            DescriptionErrorLabel.IsVisible = true;
            isValid = false;
        }
        else if (DescriptionEditor.Text.Length > 2000)
        {
            DescriptionErrorLabel.Text = "課題の文面は2000文字以内で入力してください";
            DescriptionErrorLabel.IsVisible = true;
            isValid = false;
        }

        return isValid;
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}