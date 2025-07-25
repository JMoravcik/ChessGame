namespace ChessGame.NativeApp.Platforms.iOS;

public class iOSNativeFeatures : INativeFeatures
{
    public Task<string> GetMACAddress()
    {
        throw new NotImplementedException();
    }
}
