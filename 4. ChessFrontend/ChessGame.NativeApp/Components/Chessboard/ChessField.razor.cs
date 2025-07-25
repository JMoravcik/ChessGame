using ChessGame.Common.Extensions;
using Microsoft.AspNetCore.Components;
namespace ChessGame.NativeApp.Components.Chessboard;

public partial class ChessField : ComponentBase
{
    [Parameter] public required int Row { get; set; }
    [Parameter] public required int Column { get; set; }
    [Parameter] public required int PieceId { get; set; }
    [Parameter] public required EventCallback<string> Clicked { get; set; }
    [Parameter] public bool Selectable { get; set; } = false;
    [Parameter] public bool Selected { get; set; } = false;

    private int GetX() => Column * 40;
    private int GetY() => Row * 40;
    private string GetColor()
    {
        if (Selected)
            return "#EC9706";

        return (Row + Column) % 2 == 0 ? "#f0d9b5" : "#b58863";
    }

    private int GetPieceX() => GetX() + 5;
    private int GetPieceY() => GetY() + 5;
    private bool FieldContainsPiece(out string pieceImagePath)
    {
        pieceImagePath = PieceId switch
        {
            1 => "/images/chessPieces/White_Pawn.svg",
            2 => "/images/chessPieces/White_Rook.svg",
            3 => "/images/chessPieces/White_Knight.svg",
            4 => "/images/chessPieces/White_Bishop.svg",
            5 => "/images/chessPieces/White_Queen.svg",
            6 => "/images/chessPieces/White_King.svg",
            7 => "/images/chessPieces/Black_Pawn.svg",
            8 => "/images/chessPieces/Black_Rook.svg",
            9 => "/images/chessPieces/Black_Knight.svg",
            10 => "/images/chessPieces/Black_Bishop.svg",
            11 => "/images/chessPieces/Black_Queen.svg",
            12 => "/images/chessPieces/Black_King.svg",
            _ => string.Empty,
        };

        return string.IsNullOrEmpty(pieceImagePath) == false;
    }

    private string GetClasses()
    {
        if (Selected)
            return "selected";
        if (Selectable)
            return "selectable";
        return string.Empty;
    }

    private void OnClick()
    {
        Clicked.InvokeAsync((Row, Column).ToChessNotation());
    }
}
