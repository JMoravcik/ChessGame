namespace ChessGame.ChessService.Contracts.HubMessages;

public class AuthenticationInfo
{
    public required string DeviceToken { get; set; }
    public required string UserToken { get; set; }
}
