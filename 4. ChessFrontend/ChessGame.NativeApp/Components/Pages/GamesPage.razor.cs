using ChessGame.NativeApp.Components.Chessboard;
using ChessGame.NativeApp.Components.FlowControl;
using ChessGame.NativeApp.Components.Layout;
using ChessGame.NativeApp.Managers;
using Microsoft.AspNetCore.Components;

namespace ChessGame.NativeApp.Components.Pages;

public partial class GamesPage : IInitializable
{
    [CascadingParameter] public required IModalManager ModalManager { get; set; }
    [Inject] public required GameplayManager GameplayManager { get; set; }

    public async Task InitializeAsync()
    {
        await GameplayManager.GetGamesAsync();
    }
}
