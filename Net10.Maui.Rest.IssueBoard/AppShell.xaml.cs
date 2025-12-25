using Net10.Maui.Rest.IssueBoard.Views;

namespace Net10.Maui.Rest.IssueBoard
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(IssueDetailPage), typeof(IssueDetailPage));
            Routing.RegisterRoute(nameof(IssueEditPage), typeof(IssueEditPage));
            Routing.RegisterRoute(nameof(IssueDeletePage), typeof(IssueDeletePage));
            Routing.RegisterRoute(nameof(IssueCreatePage), typeof(IssueCreatePage));
        }
    }
}
