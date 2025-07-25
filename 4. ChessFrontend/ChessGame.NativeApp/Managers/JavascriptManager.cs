using Microsoft.JSInterop;

namespace ChessGame.NativeApp.Managers;

public class JavascriptManager
{
    private readonly IJSRuntime _jsRuntime;

    public JavascriptManager(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task AlertAsync(string message)
    {
        await _jsRuntime.InvokeVoidAsync("alert", message);
    }
}
