using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace ChessGame.Common.Extensions;

public static class HeaderExtensions
{
    const string DeviceTokenKey = "Device-Token";
    const string AuthTokenKey = "Auth-Token";
    public static string? GetDeviceToken(this IHeaderDictionary headerDictionary)
    {
        return headerDictionary.ContainsKey(DeviceTokenKey) ? headerDictionary[DeviceTokenKey].ToString()
                                                            : null;
    }

    public static void SetDeviceToken(this HttpRequestHeaders headerDictionary, string deviceToken)
    {
        headerDictionary.Add(DeviceTokenKey, deviceToken);
    }

    public static string? GetAuthToken(this IHeaderDictionary headerDictionary, string deviceId)
    {
        return headerDictionary.ContainsKey(AuthTokenKey) ? headerDictionary[AuthTokenKey].ToString()
                                                            : null;
    }

    public static void SetAuthToken(this HttpRequestHeaders headerDictionary, string authToken)
    {
        headerDictionary.Add(AuthTokenKey, authToken);
    }

    public static string CreateAuthToken(this Guid profileId) => profileId.ToString();

}
