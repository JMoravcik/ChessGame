using Android.App;
using Android.Runtime;
using ChessGame.NativeApp.Platforms.Android;

namespace ChessGame.NativeApp
{
    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp<AndroidNativeFeatures>();
    }
}
