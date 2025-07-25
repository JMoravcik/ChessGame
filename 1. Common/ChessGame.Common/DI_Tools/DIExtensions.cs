using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Common.DI_Tools;


public static class DendenciesExtensions
{
    private static List<Setup>? _setups = null;

    public static IServiceCollection SetupDependencies(this IServiceCollection services, EnvironmentInfo environmentInfo)
    {
        _setups ??= Setup.GetSetups<Setup>(environmentInfo).ToList();
        Setup.CollectDependencies(_setups, services);
        return services;
    }

    public static IHost StartupDependencies(this IHost host)
    {
        Setup.StartupModuls(_setups!, host);
        return host;
    }

    public static async Task WarmUpAsync(this IServiceProvider serviceProvider)
    {
        foreach (var warmUpOperation in serviceProvider.GetServices<IWarmupOperation>().OrderBy(warmUp => warmUp.OrderNumber))
        {
            await warmUpOperation.WarmUpAsync();
        }
    }

    public static IServiceCollection AddWarmupSingleton<TInterface, TImplementation>(this IServiceCollection serviceDescriptors)
        where TImplementation : class, TInterface, IWarmupOperation
        where TInterface : class
    {
        serviceDescriptors.AddSingleton<TImplementation>();
        serviceDescriptors.AddSingleton<TInterface, TImplementation>(sp => sp.GetRequiredService<TImplementation>());
        serviceDescriptors.AddSingleton<IWarmupOperation>(sp => sp.GetRequiredService<TImplementation>());
        return serviceDescriptors;
    }
}
