using System.Reflection;

namespace ChessGame.Common.Utils;

public static class AssemblyUtils
{
    private static List<Assembly>? _assemblies = null;
    internal static List<Assembly> Assemblies => _assemblies ??= LoadAssemblies();

    private static List<Assembly> LoadAssemblies()
    {
        if (GetProjectBaseName() is not string projectBaseName)
            return new();

        bool IsRelevantAssembly(string? fullName)
            => fullName != null && (fullName.StartsWith(projectBaseName));

        var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => IsRelevantAssembly(a.FullName)).ToList();
        var loadedPaths = loadedAssemblies.Select(a => a.Location).ToArray();


        var referencedPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
                                       .Where(file => IsRelevantAssembly(file.Split(new char[] { '\\', '/' }).Last()));

        var toLoad = referencedPaths.Where(r => !loadedPaths.Contains(r, StringComparer.InvariantCultureIgnoreCase)).ToList();

        toLoad.ForEach(path => loadedAssemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(path))));
        return loadedAssemblies;
    }


    public static IEnumerable<Assembly> GetFrontendAssemblies(params Assembly[] ignoredAssemblies)
    {
        if (GetProjectBaseName() is not string projectBaseName)
            return new List<Assembly>();

        bool IsRelevantAssembly(string? fullName)
            => fullName != null && (fullName.StartsWith(projectBaseName) && fullName.Contains("Frontend"));

        var result = (from assembly in AssemblyUtils.Assemblies
                      where IsRelevantAssembly(assembly.FullName)
                      where !ignoredAssemblies.Contains(assembly)
                      select assembly).ToList();
        return result;
    }

    public static List<Type> GetAllTypes() => Assemblies.Select(assembly => assembly.GetTypes()).SelectMany(t => t).ToList();

    public static Type GetTypeFromAssemblies(string fullName)
    {
        foreach (var assembly in Assemblies)
        {
            var type = assembly.GetType(fullName);
            if (type != null)
                return type;
        }
        var listAssembly = typeof(List<>).Assembly;
        var listType = listAssembly.GetType(fullName);
        if (listType != null)
            return listType;

        throw new Exception($"Couldn't get type with full name '{fullName}'");
    }
    private static string? GetProjectBaseName()
    {
        var entryAssembly = Assembly.GetExecutingAssembly();
        if (entryAssembly?.FullName is not string fullName)
            return null;

        return fullName.Split('.').FirstOrDefault();
    }


    public static IEnumerable<Type> GetDerivedClasses<TClass>()
    {
        var classType = typeof(TClass);
        var classAssembly = classType.Assembly;
        foreach (var type in classAssembly.GetTypes())
        {
            if (type.IsDerivedFrom(classType))
                yield return type;
        }
    }

    public static IEnumerable<Type> GetClassesByInterface<TInterface>() => GetClassesByInterface(typeof(TInterface).Assembly, typeof(TInterface).Name);

    public static IEnumerable<Type> GetClassesByInterface(Assembly interfacesAssembly, string interfaceName)
        => from classType in interfacesAssembly.GetTypes()
           where classType.GetInterface(interfaceName) != null
           select classType;

    private static bool IsDerivedFrom(this Type type, Type baseType)
    {
        if (type.BaseType == baseType)
            return true;
        else if (type.BaseType == null)
            return false;
        else
            return type.BaseType.IsDerivedFrom(baseType);
    }

    public static TInstance CreateInstance<TInstance>(Type type, params object[] constructorArgs)
    {
        return (TInstance)Activator.CreateInstance(type, constructorArgs)!;
    }

    public static Type CreateConcreeteType(Type genericType, params Type[] typeArgs)
    {
        return genericType.MakeGenericType(typeArgs);
    }
}
