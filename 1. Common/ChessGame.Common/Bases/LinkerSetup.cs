using ChessGame.Common.DI_Tools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Common.Bases;

public abstract class LinkerSetup : Setup
{
    private readonly bool _asSingleton;

    public LinkerSetup(EnvironmentInfo environment, string configSection) : base(environment)
    {
        _asSingleton = environment.Configuration.GetValue($"{configSection}:AsSingleton", true);
    }

    protected void RegisterLinker<TInterface, TLinker>(IServiceCollection serviceCollection)
        where TLinker : Linker, TInterface
        where TInterface : class
    {
        if (_asSingleton)
            serviceCollection.AddSingleton<TInterface, TLinker>();
        else
            serviceCollection.AddScoped<TInterface, TLinker>();
    }

    protected bool IsSameApiLinkersAreReferencingTo(string recognizingNameString)
    {
        string assemblyName = Assembly.GetEntryAssembly()?.FullName ?? string.Empty;
        return assemblyName.Contains(recognizingNameString);
    }
}
