namespace ChessGame.Common.Extensions;

public static class IntExtensions
{
    public static bool IsInRange(this int value, int min, int max)
    {
        return value >= min && value <= max;
    }
}
