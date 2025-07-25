namespace ChessGame.Common.Entities.Identity;

public class UserProfile : Entity
{
    public required string NickName { get; set; }
    public required string Email { get; set; }
}
