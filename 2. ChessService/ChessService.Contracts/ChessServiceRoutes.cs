namespace ChessGame.ChessService.Contracts;

public static class ChessServiceRoutes
{
    public class Chess
    {
        public const string BaseServiceUrl = "chess";

        public const string CreateGame = "create-game";
        public const string GetMinimap = "get-minimap/{gameId}";
    }
}
