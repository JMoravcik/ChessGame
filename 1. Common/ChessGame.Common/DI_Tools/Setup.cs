using ChessGame.Common.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChessGame.Common.DI_Tools;

public abstract class Setup
{
    protected EnvironmentInfo Environment { get; }

    public Setup(EnvironmentInfo environment)
    {
        Environment = environment;
    }

    public virtual bool ShouldSetup(params object[] args) => true;
    public virtual void AddDependencies(IServiceCollection serviceCollection, params object[] args) { }
    public virtual void Startup(IHost host, params object[] args) { }

    #region Static Functions

    public static IEnumerable<TInstance> GetSetups<TInstance>(EnvironmentInfo environmentInfo)
        where TInstance : Setup
        => from assembly in AssemblyUtils.Assemblies
           from type in assembly.GetTypes()
           where HasBaseType(type, typeof(TInstance)) && !type.IsAbstract
           select CreateInstance<TInstance>(type, environmentInfo)!;

    private static TInstance CreateInstance<TInstance>(Type type, EnvironmentInfo environmentInfo)
        where TInstance : Setup
    {
        return (TInstance)Activator.CreateInstance(type, environmentInfo)!;
    }

    private static bool HasBaseType(Type type, Type baseType)
    {
        if (type.BaseType == null)
            return false;
        else if (type.BaseType == baseType)
            return true;
        else
            return HasBaseType(type.BaseType, baseType);
    }

    public static void CollectDependencies(IEnumerable<Setup> setups, IServiceCollection serviceCollection, params object[] args)
    {
        foreach (var setup in setups)
        {
            if (setup.ShouldSetup(args))
                setup.AddDependencies(serviceCollection, args);
        }
    }

    public static void StartupModuls(IEnumerable<Setup> setups, IHost host, params object[] args)
    {
        foreach (var setup in setups)
        {
            if (setup.ShouldSetup(host, args))
                setup.Startup(host, args);
        }
    }

    #endregion

}
