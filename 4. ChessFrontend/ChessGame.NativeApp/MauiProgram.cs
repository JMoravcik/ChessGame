using ChessGame.Common.Bases;
using ChessGame.Common.DI_Tools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ChessGame.NativeApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp<TNativeFeatures>()
            where TNativeFeatures : class, INativeFeatures
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Configuration.AddJsonFile("appsettings.json");
            builder.Services.AddSingleton<INativeFeatures, TNativeFeatures>();
            builder.Services.AddMauiBlazorWebView();

            builder.Services.AddSingleton<NativeAppLinkerContext>();
            builder.Services.AddSingleton<LinkerContext>(sp => sp.GetRequiredService<NativeAppLinkerContext>());

            builder.Services.SetupDependencies(new EnvironmentInfo(builder.Configuration));
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif
            var app = builder.Build();
            return app;
        }
    }
}
