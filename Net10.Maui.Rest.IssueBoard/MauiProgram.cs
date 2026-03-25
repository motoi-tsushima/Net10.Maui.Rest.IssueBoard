using Microsoft.Extensions.Logging;
using Net10.Maui.Rest.IssueBoard.Services;
using Net10.Maui.Rest.IssueBoard.Views;

namespace Net10.Maui.Rest.IssueBoard
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<HttpClient>(sp =>
            {
                var handler = new HttpClientHandler
                {
#if DEBUG
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
#endif
                };
                
                string baseUrl;
#if ANDROID
                // Androidエミュレータからホストマシンにアクセスする場合は10.0.2.2を使用
                baseUrl = "http://10.0.2.2:5000"; 
#elif IOS
                // iOSシミュレーター（Mac上で動作）からWindowsのAPIサーバーにアクセス
                baseUrl = "http://192.168.1.9:5000";
#else
                baseUrl = "https://localhost:7001";
#endif
                
                return new HttpClient(handler)
                {
                    BaseAddress = new Uri(baseUrl),
                    Timeout = TimeSpan.FromSeconds(30)
                };
            });

            builder.Services.AddSingleton<IssueService>();

            builder.Services.AddTransient<IssueListPage>();
            builder.Services.AddTransient<IssueDetailPage>();
            builder.Services.AddTransient<IssueEditPage>();
            builder.Services.AddTransient<IssueDeletePage>();
            builder.Services.AddTransient<IssueCreatePage>();

            return builder.Build();
        }
    }
}
