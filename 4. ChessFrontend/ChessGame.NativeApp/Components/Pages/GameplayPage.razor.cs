using ChessGame.ChessService.Contracts.HubLinker;
using ChessGame.ChessService.Contracts.IHub;
using ChessGame.ChessService.Contracts.MoveResults;
using ChessGame.NativeApp.Components.Chessboard;
using ChessGame.NativeApp.Components.FlowControl;
using ChessGame.NativeApp.Components.Layout;
using ChessGame.NativeApp.Managers;
using Microsoft.AspNetCore.Components;

namespace ChessGame.NativeApp.Components.Pages;

public partial class GameplayPage : ComponentBase, IInitializable, IDisposable
{
    [CascadingParameter] public required IModalManager ModalManager { get; set; }
    [Inject] public required ChessEvents ChessEvents { get; set; }
    [Inject] public required IChessGameHub ChessGameHub { get; set; }
    [Inject] public required GameplayManager GameplayManager { get; set; }
    [Inject] public required JavascriptManager JavascriptManager { get; set; }

    public async Task InitializeAsync()
    {
        await GameplayManager.GetGameStateAsync();
        ChessEvents.SubscribeToMoveResult(this, OnMoveResultAsync);
    }

    private async void OnMoveResultAsync(MoveResult moveResult)
    {
        try //vrací se z nějakého důvodu finish move a neswitchuje se kolo černého a bílého
        {
            if (moveResult is InvalidMove invalidMove)
            {
                await JavascriptManager.AlertAsync(invalidMove.Reason);
            }
            else
            {
                await RefreshAsync();
            }

        }
        catch (Exception e)
        {
            throw new Exception($"{OnMoveResultAsync} threw exception!", e);
        }
    }

    private async Task RefreshAsync()
    {
        await GameplayManager.GetGameStateAsync();
        _ = InvokeAsync(StateHasChanged);
    }

    private async Task OnMoveSelectAsync(string move)
    {
        if (move.Contains("PromotionMove"))
        {
            ModalManager.Show<PromotionModal>(ChessGameNativeAppRes.PromotionModal_Title, new()
            {
                { nameof(PromotionModal.PromotionMove), move }
            });
        }
        else
        {
            await ChessGameHub.MovePieceAsync(move);
        }
    }

    public void Dispose()
    {
        ChessEvents.Unsubscribe(this);
    }
}
