using ChessGame.Common.Entities.Identity;

namespace ChessGame.AccessService.Contracts.Requests.Authentication;

public class RegisterUserProfileRequest
{
    public required UserProfile UserProfile { get; set; }
    public required BasicAuth BasicAuth { get; set; }
}
