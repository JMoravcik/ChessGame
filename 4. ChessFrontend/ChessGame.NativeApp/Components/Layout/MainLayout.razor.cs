using ChessGame.NativeApp.Components.FlowControl;
using ChessGame.NativeApp.Managers;
using Microsoft.AspNetCore.Components;

namespace ChessGame.NativeApp.Components.Layout;

public partial class MainLayout : IInitializable
{
    [Inject] public required AuthorizationManager AuthorizationManager { get; set; }
    public async Task InitializeAsync()
    {
        await AuthorizationManager.InitializeAsync();
    }
}
