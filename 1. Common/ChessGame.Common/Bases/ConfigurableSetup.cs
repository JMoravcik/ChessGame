using ChessGame.Common.DI_Tools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ChessGame.Common.Bases;

public abstract class ConfigurableSetup : Setup
{
    private readonly bool _asSingleton;

    public ConfigurableSetup(EnvironmentInfo environment, string configSection) : base(environment)
    {
        _asSingleton = environment.Configuration.GetValue($"{configSection}:AsSingleton", true);
    }

    protected void Register<TInterface, TLinker>(IServiceCollection serviceCollection)
        where TLinker : class, TInterface
        where TInterface : class
    {
        if (_asSingleton)
            serviceCollection.AddSingleton<TInterface, TLinker>();
        else
            serviceCollection.AddScoped<TInterface, TLinker>();
    }

    protected void Register<TImplementation>(IServiceCollection serviceCollection)
        where TImplementation : class
    {
        if (_asSingleton)
            serviceCollection.AddSingleton<TImplementation>();
        else
            serviceCollection.AddScoped<TImplementation>();
    }

    protected bool IsSameApiLinkersAreReferencingTo(string recognizingNameString)
    {
        string assemblyName = Assembly.GetEntryAssembly()?.FullName ?? string.Empty;
        return assemblyName.Contains(recognizingNameString);
    }
}
