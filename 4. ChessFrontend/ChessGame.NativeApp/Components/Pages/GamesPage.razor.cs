using ChessGame.NativeApp.Components.FlowControl;
using ChessGame.NativeApp.Managers;
using Microsoft.AspNetCore.Components;

namespace ChessGame.NativeApp.Components.Pages;

public partial class GamesPage : IInitializable
{
    [Inject] public required GameplayManager GameplayManager { get; set; }

    public Task InitializeAsync()
    {
        return GameplayManager.GetGamesAsync();
    }
}
