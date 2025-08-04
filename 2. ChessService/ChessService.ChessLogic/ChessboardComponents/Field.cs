using ChessGame.ChessService.ChessLogic.Pieces;
using ChessGame.Common.Extensions;

namespace ChessGame.ChessService.ChessLogic.ChessboardComponents;

public class Field
{
    public int Row { get; }
    public int Column { get; }
    public Piece? Piece { get; set; }
    public List<bool> ThreatenedBy { get; } = new List<bool>();

    public bool IsOccupied => Piece != null;

    public Field(int row, int column, Piece? piece = null)
    {
        Row = row;
        Column = column;
        Piece = piece;
    }

    public bool IsThreatenedFor(bool isWhite)
        => ThreatenedBy.Contains(isWhite);

    public string GetFieldCoordinate() => (Row, Column).ToChessNotation();

    public static (int Row, int Column) ParseFieldCoordinate(string coordinate) => coordinate.FromChessNotation();

    public override string ToString()
    {
        return GetFieldCoordinate();
    }
}
