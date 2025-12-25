namespace Net10.Maui.Rest.IssueBoard.Helpers;

public static class PreferencesHelper
{
    private const string AuthorNameKey = "AuthorName";

    public static void SaveAuthorName(string authorName)
    {
        Preferences.Set(AuthorNameKey, authorName);
    }

    public static string GetAuthorName()
    {
        return Preferences.Get(AuthorNameKey, string.Empty);
    }
}
