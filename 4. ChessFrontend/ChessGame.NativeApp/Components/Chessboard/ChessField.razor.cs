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
        pieceImagePath = GetImageByPieceId(PieceId);
        return string.IsNullOrEmpty(pieceImagePath) == false;
    }

    public static string GetImageByPieceId(int pieceId) => pieceId switch
    {
        1 => ChessGameNativeAppRes.ChessPieceImg_WhitePawn,
        2 => ChessGameNativeAppRes.ChessPieceImg_WhiteRook,
        3 => ChessGameNativeAppRes.ChessPieceImg_WhiteKnight,
        4 => ChessGameNativeAppRes.ChessPieceImg_WhiteBishop,
        5 => ChessGameNativeAppRes.ChessPieceImg_WhiteQueen,
        6 => ChessGameNativeAppRes.ChessPieceImg_WhiteKing,
        7 => ChessGameNativeAppRes.ChessPieceImg_BlackPawn,
        8 => ChessGameNativeAppRes.ChessPieceImg_BlackRook,
        9 => ChessGameNativeAppRes.ChessPieceImg_BlackKnight,
        10 => ChessGameNativeAppRes.ChessPieceImg_BlackBishop,
        11 => ChessGameNativeAppRes.ChessPieceImg_BlackQueen,
        12 => ChessGameNativeAppRes.ChessPieceImg_BlackKing,
        _ => string.Empty,
    };

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
