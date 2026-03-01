using System.Reflection;
using Lar.TesteDotNet.Domain.Common;

namespace Lar.TesteDotNet.Tests.Shared.Extensions;

public static class EntityExtensions
{
    public static void GenerateIds(this Entity entity, long value = 1)
    {
        var publicProperties = entity
            .GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance);

        var idField = typeof(Entity)
            .GetField(
                "<Id>k__BackingField",
                BindingFlags.Instance | BindingFlags.NonPublic);

        idField?.SetValue(entity, value);

        foreach (var property in publicProperties)
        {
            var currentType = property.PropertyType;

            if (currentType.BaseType == typeof(Entity))
            {
                var propertyValue = property.GetValue(entity);

                if (propertyValue is null) continue;

                GenerateIds((Entity)propertyValue);
            }

            if (currentType.IsGenericType && currentType.GetGenericTypeDefinition() == typeof(IReadOnlyCollection<>))
            {
                if (currentType.GenericTypeArguments[0].BaseType != typeof(Entity)) continue;

                var propertyValue = property.GetValue(entity);

                if (propertyValue is null) continue;

                var list = (IReadOnlyCollection<Entity>)propertyValue;

                var count = 0;
                foreach (var item in list)
                {
                    count++;
                    GenerateIds(item, count);
                }
            }

            if (!property.Name.EndsWith("Id")) continue;

            if (currentType != typeof(long) && currentType != typeof(long?)) continue;

            if (property.SetMethod is null) continue;

            property.SetValue(entity, 1L);
        }
    }
}