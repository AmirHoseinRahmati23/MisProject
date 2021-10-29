using ClientLibraries.Security;

namespace Core.Security;

public static class SecurityIgnoredMethod<Type>
{
    public static Type RemoveUnSecureParameters(Type obj)
    {
        var propertyInfos = typeof(Type).GetProperties();
        foreach (var propertyInfo in propertyInfos)
        {
            if (propertyInfo.CustomAttributes.Any(p => p.AttributeType == typeof(SecurityIgnoredAttribute)))
            {
                propertyInfo.SetValue(obj, Convert.ChangeType(null, propertyInfo.PropertyType), null);
            }
        }

        return obj;
    }
}