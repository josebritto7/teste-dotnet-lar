using System.Reflection;
using Mapster;

namespace Lar.TesteDotNet.Presentation.Configurations.Mapping;

public class MappingConfiguration
{
    private MappingConfiguration()
    {
        var iface = typeof(IMapFrom<>);
        ApplyMappingsFromAssembly(iface.Assembly, iface);
    }

    public static void ConfigureMapping()
    {
        _ = new MappingConfiguration();
    }

    private void ApplyMappingsFromAssembly(Assembly assembly, Type interfaceType)
    {
        var assemblyTypes = assembly.GetExportedTypes();

        AddMappingForTypes(assemblyTypes, interfaceType);
    }

    private static void AddMappingForTypes(IEnumerable<Type> types, Type interfaceType)
    {
        var filteredTypes = types
            .Where(t => HasInterface(t, interfaceType))
            .Where(t => t.BaseType == null || !HasInterface(t.BaseType, interfaceType));

        foreach (var type in filteredTypes) AddMappingForType(type, interfaceType.Name);
    }

    private static void AddMappingForType(Type type, string interfaceName)
    {
        const string methodName = "Mapping";
        var instance = Activator.CreateInstance(type);
        var methodInfo = type.GetMethod(methodName) ?? type.GetInterface(interfaceName)?.GetMethod(methodName);
        methodInfo?.Invoke(instance, new object[] { TypeAdapterConfig.GlobalSettings });
    }

    private static bool HasInterface(Type sourceType, Type interfaceType)
    {
        return sourceType.GetInterfaces()
            .Any(i => i.IsGenericType &&
                      i.GetGenericTypeDefinition() == interfaceType);
    }
}