using System.Data.Common;

namespace ChessGame.Common.Extensions;

public static class ChessExtensions
{
    public static string ToChessNotation(this (int Row, int Column) position)
    {
        char columnChar = (char)('a' + position.Column);
        int rowNumber = position.Row + 1; // Convert to chess notation (1-8)
        return $"{columnChar}{rowNumber}";
    }

    public static (int Row, int Column) FromChessNotation(this string notation)
    {
        int column = notation[0] - 'a';
        int row = notation[1] - '1';

        return (row, column);
    }
}
