namespace ChessGame.AccessService.Contracts;

public static class AccessServiceRoutes
{
    public static class Authentication
    {
        public const string BaseServiceUrl = "authentication";
        public const string RegisterUserProfile = "register-user-profile";
        public const string IsDeviceRegistered = "is-device-registered";
        public const string RegisterDevice = "register-device";
        public const string BasicLogin = "basic-login";
        public const string BasicLogout = "basic-logout";
        public const string GetProfileByDeviceAndToken = "get-profile-by-device-and-token";
        public const string IsLoggedIn = "is-logged-in";
    }

    public static class GameAccess
            {
        public const string BaseServiceUrl = "ChessAccess";
        public const string CreateGame = "create-game";
        public const string GetGames = "get-games";
        public const string GetCurrentGameRecord = "get-current-game-record";
    }
}
