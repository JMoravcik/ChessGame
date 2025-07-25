using ChessGame.Backend.Common.Scopeds.Database.DbResults;
using ChessGame.Common.Entities.Identity;

namespace ChessGame.Backend.Common.Scopeds.Database;

public interface IAuthenticationRepository
{
    Task<UserProfile?> GetProfileByDeviceAndTokenAsync(Guid deviceId, string token);

    Task<bool> RegisterDeviceAsync(Guid id);

    Task<string?> RegisterUserAsync(UserProfile userProfile, BasicAuth basicAuth);

    Task<bool> LogoutAsync(Guid deviceId, string token);

    Task<LoginDbResult?> BasicLoginAsync(Guid deviceId, string passwordHash);

    Task<Device?> GetDeviceByIdAsync(Guid deviceId);

}
