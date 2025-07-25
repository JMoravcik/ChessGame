using ChessGame.NativeApp.Components.FlowControl;
using ChessGame.NativeApp.Managers;
using Microsoft.AspNetCore.Components;

namespace ChessGame.NativeApp.Components.Pages;

public partial class GameplayPage : ComponentBase, IInitializable
{
    [Inject] public required GameplayManager GameplayManager { get; set; }
    public async Task InitializeAsync()
    {
        await GameplayManager.GetGameStateAsync();
    }
}
