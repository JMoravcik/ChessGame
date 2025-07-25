using ChessGame.Common.Entities.Identity;

namespace ChessGame.AccessService.Contracts.Responses.Authentication;

public class AuthenticatedResponse
{
    public required UserProfile UserProfile { get; set; }
    public required string Token { get; set; }
}
