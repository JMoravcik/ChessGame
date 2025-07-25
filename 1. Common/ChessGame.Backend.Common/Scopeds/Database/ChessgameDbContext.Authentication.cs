using ChessGame.Backend.Common.Scopeds.Database;
using ChessGame.Backend.Common.Scopeds.Database.DbResults;
using ChessGame.Common.Entities.Identity;
using ChessGame.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ChessGame.Backend.Common.Scopeds.Database;

public partial class ChessgameDbContext : IAuthenticationRepository
{
    public async Task<LoginDbResult?> BasicLoginAsync(Guid deviceId, string passwordHash)
    {
        var basicAuth = await BasicAuths.FirstOrDefaultAsync(basicAuth => basicAuth.PasswordHash == passwordHash);
        if (basicAuth == null)
        {
            _logger.LogWarning("BasicAuth not found for password hash: {PasswordHash}", passwordHash);
            return null;
        }

        var device = await Devices.FirstOrDefaultAsync(device => device.Id == deviceId);
        if (device == null)
        {
            _logger.LogWarning("Device not found for ID: {DeviceId}", deviceId);
            return null;
        }

        var userProfile = await UserProfiles.FirstOrDefaultAsync(up => up.Id == basicAuth.UserProfileId);
        if (userProfile == null)
        {
            _logger.LogWarning($"UserProfile not found for BasicAuth '{basicAuth.Id}' with ID: {basicAuth.UserProfileId}");
            return null;
        }

        var deviceToken = await SetDeviceLoginInformationAsync(device, userProfile);

        return new LoginDbResult()
        {
            UserProfile = userProfile,
            Token = deviceToken
        };
    }

    private async Task<string> SetDeviceLoginInformationAsync(Device device, UserProfile userProfile)
    {
        device.Token = DateTime.UtcNow.ToString().ToSHA256();
        device.LoggedUserProfileId = userProfile.Id;
        await SaveChangesAsync();
        return device.Token;
    }

    public async Task<UserProfile?> GetProfileByDeviceAndTokenAsync(Guid deviceId, string token)
    {
        var device = await Devices
            .FirstOrDefaultAsync(d => d.Id == deviceId && d.Token == token);
        if (device == null)
        {
            _logger.LogWarning("Device not found for ID: {DeviceId} with token: {Token}", deviceId, token);
            return null;
        }

        var userProfile = await UserProfiles.FirstOrDefaultAsync(up => up.Id == device.LoggedUserProfileId);
        return userProfile;
    }

    public async Task<bool> LogoutAsync(Guid deviceId, string token)
    {

        var device = await Devices.FirstOrDefaultAsync(device => device.Id == deviceId && device.Token == token);
        if (device == null)
        {
            _logger.LogWarning("Device not found for ID: {DeviceId}", deviceId);
            return false;
        }

        device.LoggedUserProfileId = null;
        device.Token = null;
        await SaveChangesAsync();
        return true;
    }

    public async Task<bool> IsDeviceRegisteredAsync(Guid deviceId) => await Devices.FirstOrDefaultAsync(d => d.Id == deviceId) == null;

    public async Task<bool> RegisterDeviceAsync(Guid id)
    {
        if (Devices.Any(d => d.Id == id))
            return false;

        Devices.Add(new Device { Id = id });
        await SaveChangesAsync();
        return true;
    }

    public async Task<string?> RegisterUserAsync(UserProfile userProfile, BasicAuth basicAuth)
    {
        if (UserProfiles.Any(up => up.NickName == userProfile.NickName))
        {

            return ChessGameBackendCommonRes.DbCreateUserProfile_NicknameAlreadyExists.FillTemplate(userProfile.NickName);
        }
        if (UserProfiles.Any(up => up.Email == userProfile.Email))
        {
            return ChessGameBackendCommonRes.DbCreateUserProfile_EmailAlreadyExists.FillTemplate(userProfile.Email);
        }

        UserProfiles.Add(userProfile);
        BasicAuths.Add(basicAuth);
        
        await SaveChangesAsync();
        return null;
    }

    public async Task<Device?> GetDeviceByIdAsync(Guid deviceId)
    {
        var device = await Devices.FirstOrDefaultAsync(device => device.Id == deviceId);
        return device;
    }
}
