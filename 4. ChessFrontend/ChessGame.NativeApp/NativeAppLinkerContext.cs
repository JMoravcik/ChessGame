using ChessGame.Common.Bases;
using ChessGame.Common.Extensions;

namespace ChessGame.NativeApp;

public class NativeAppLinkerContext : LinkerContext
{
    private readonly INativeFeatures _nativeFeatures;

    public NativeAppLinkerContext(INativeFeatures nativeFeatures)
    {
        _nativeFeatures = nativeFeatures;
    }
    private string? _deviceToken { get; set; } = null;
    private string? _authToken { get; set; } = null;

    public override Task<string?> GetAuthTokenAsync()
    {
        return Task.FromResult(_authToken);
    }

    public override async Task<string> GetDeviceTokenAsync()
    {
        _deviceToken ??= (await _nativeFeatures.GetMACAddress()).ToHashGuid().ToString();
        return _deviceToken;
    }

    public void SetAuthToken(string? authToken)
    {
        _authToken = authToken;
    }
}

