using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Doerly.Host.ModelBinders.FloatingPoint;

public class FloatingPointModelBinderProvider : IModelBinderProvider
{
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
        var type = context.Metadata.ModelType;

        if (type == typeof(float) || type == typeof(float?) ||
            type == typeof(double) || type == typeof(double?) ||
            type == typeof(decimal) || type == typeof(decimal?))
        {
            var binderType = typeof(FloatingPointModelBinder<>).MakeGenericType(type);
            return (IModelBinder)Activator.CreateInstance(binderType);
        }

        return null;
    }
}
