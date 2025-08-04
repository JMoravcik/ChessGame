using ChessGame.ChessService.Contracts.IHub;
using ChessGame.ChessService.Contracts.IServices;
using ChessGame.NativeApp.Components.Layout;
using ChessGame.NativeApp.Managers;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessGame.NativeApp.Components.Chessboard;

public partial class PromotionModal : ComponentBase
{
    [Inject] public required GameplayManager GameplayManager { get; set; }
    [Inject] public required IChessGameHub ChessGameHub { get; set; }
    [CascadingParameter] public required IModalManager ModalManager { get; set; }
    [Parameter] public required string PromotionMove { get; set; }

    private IEnumerable<int> GetPromotionOptions()
    {
        if (GameplayManager.GameState?.WhiteOnTurn == true)
        {
            yield return 2;
            yield return 3;
            yield return 4;
            yield return 5;
        }
        else
        {
            yield return 8;
            yield return 9;
            yield return 10;
            yield return 11;
        }
    }

    private async Task SendPromotionToPieceAsync(int pieceId)
    {
        string sendMove = $"{PromotionMove} {pieceId}";
        await ChessGameHub.MovePieceAsync(sendMove);
        ModalManager.Hide();
    }
}
