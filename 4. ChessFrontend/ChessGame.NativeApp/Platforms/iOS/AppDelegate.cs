using ChessGame.NativeApp.Platforms.iOS;
using Foundation;

namespace ChessGame.NativeApp
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp<iOSNativeFeatures>();
    }
}
