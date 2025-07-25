using System.Net.NetworkInformation;

namespace ChessGame.NativeApp.Platforms.Windows;

public class WindowsNativeFeatures : INativeFeatures
{
    public Task<string> GetMACAddress()
    {

        var mac = NetworkInterface
            .GetAllNetworkInterfaces()
            .Where(nic => nic.OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Up
                          && nic.NetworkInterfaceType != System.Net.NetworkInformation.NetworkInterfaceType.Loopback)
            .Select(nic => nic.GetPhysicalAddress()?.ToString())
            .FirstOrDefault(addr => !string.IsNullOrEmpty(addr));
        if (mac == null)
        {
            throw new InvalidOperationException("No active network interface found with a valid MAC address.");
        }

        return Task.FromResult(mac);
    }
}
