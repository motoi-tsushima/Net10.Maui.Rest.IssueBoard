using Net10.Maui.Rest.IssueBoard.Helpers;
using Net10.Maui.Rest.IssueBoard.Services;
using Shared.Rest.IssueBoard;

namespace Net10.Maui.Rest.IssueBoard.Views;

public partial class IssueCreatePage : ContentPage
{
    private readonly IssueService _issueService;

    public IssueCreatePage(IssueService issueService)
    {
        InitializeComponent();
        _issueService = issueService;
        LoadSavedAuthorName();
    }

    private void LoadSavedAuthorName()
    {
        var savedName = PreferencesHelper.GetAuthorName();
        if (!string.IsNullOrEmpty(savedName))
        {
            AuthorNameEntry.Text = savedName;
        }
    }

    private async void OnCreateClicked(object sender, EventArgs e)
    {
        if (!ValidateInput())
        {
            return;
        }

        try
        {
            var createDto = new CreateIssueDto
            {
                AuthorName = AuthorNameEntry.Text,
                Category = string.IsNullOrWhiteSpace(CategoryEntry.Text) ? null : CategoryEntry.Text,
                Title = TitleEntry.Text,
                Description = DescriptionEditor.Text
            };

            await _issueService.CreateIssueAsync(createDto);
            
            PreferencesHelper.SaveAuthorName(AuthorNameEntry.Text);

            await DisplayAlertAsync("¬Œ÷", "‰Û‘è‚ð“o˜^‚µ‚Ü‚µ‚½", "OK");
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("ƒGƒ‰[", $"‰Û‘è‚Ì“o˜^‚ÉŽ¸”s‚µ‚Ü‚µ‚½: {ex.Message}", "OK");
        }
    }

    private bool ValidateInput()
    {
        bool isValid = true;

        AuthorNameErrorLabel.IsVisible = false;
        TitleErrorLabel.IsVisible = false;
        DescriptionErrorLabel.IsVisible = false;

        if (string.IsNullOrWhiteSpace(AuthorNameEntry.Text))
        {
            AuthorNameErrorLabel.Text = "‹L“üŽÒŽ–¼‚Í•K{‚Å‚·";
            AuthorNameErrorLabel.IsVisible = true;
            isValid = false;
        }
        else if (AuthorNameEntry.Text.Length > 50)
        {
            AuthorNameErrorLabel.Text = "‹L“üŽÒŽ–¼‚Í50•¶ŽšˆÈ“à‚Å“ü—Í‚µ‚Ä‚­‚¾‚³‚¢";
            AuthorNameErrorLabel.IsVisible = true;
            isValid = false;
        }

        if (string.IsNullOrWhiteSpace(TitleEntry.Text))
        {
            TitleErrorLabel.Text = "‰Û‘èƒ^ƒCƒgƒ‹‚Í•K{‚Å‚·";
            TitleErrorLabel.IsVisible = true;
            isValid = false;
        }
        else if (TitleEntry.Text.Length > 100)
        {
            TitleErrorLabel.Text = "‰Û‘èƒ^ƒCƒgƒ‹‚Í100•¶ŽšˆÈ“à‚Å“ü—Í‚µ‚Ä‚­‚¾‚³‚¢";
            TitleErrorLabel.IsVisible = true;
            isValid = false;
        }

        if (string.IsNullOrWhiteSpace(DescriptionEditor.Text))
        {
            DescriptionErrorLabel.Text = "‰Û‘è‚Ì•¶–Ê‚Í•K{‚Å‚·";
            DescriptionErrorLabel.IsVisible = true;
            isValid = false;
        }
        else if (DescriptionEditor.Text.Length > 2000)
        {
            DescriptionErrorLabel.Text = "‰Û‘è‚Ì•¶–Ê‚Í2000•¶ŽšˆÈ“à‚Å“ü—Í‚µ‚Ä‚­‚¾‚³‚¢";
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