namespace ChessGame.AccessService.Contracts.Requests.Authentication;

public class BasicAuthenticationRequest
{
    public required string PasswordHash { get; set; }
}
