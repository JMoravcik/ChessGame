namespace ChessGame.NativeApp;

public interface INativeFeatures
{
    Task<string> GetMACAddress();
}
