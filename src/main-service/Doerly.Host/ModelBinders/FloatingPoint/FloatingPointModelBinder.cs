using System.Globalization;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Doerly.Host.ModelBinders.FloatingPoint;

public class FloatingPointModelBinder<T> : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
            throw new ArgumentNullException(nameof(bindingContext));

        var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
        bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

        var attemptedValue = valueProviderResult.FirstValue;

        if (string.IsNullOrWhiteSpace(attemptedValue))
        {
            if (Nullable.GetUnderlyingType(typeof(T)) != null)
            {
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;
            }

            bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "A value is required.");
            return Task.CompletedTask;
        }

        string decimalSeparator = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
        string alternateSeparator = decimalSeparator == "," ? "." : ",";

        if (attemptedValue.IndexOf(decimalSeparator) == -1 &&
            attemptedValue.IndexOf(alternateSeparator) != -1)
        {
            attemptedValue = attemptedValue.Replace(alternateSeparator, decimalSeparator);
        }

        try
        {
            object parsedValue;

            var targetType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

            if (targetType == typeof(decimal))
                parsedValue = decimal.Parse(attemptedValue, NumberStyles.Any, CultureInfo.CurrentCulture);
            else if (targetType == typeof(double))
                parsedValue = double.Parse(attemptedValue, NumberStyles.Any, CultureInfo.CurrentCulture);
            else if (targetType == typeof(float))
                parsedValue = float.Parse(attemptedValue, NumberStyles.Any, CultureInfo.CurrentCulture);
            else
                throw new NotSupportedException($"Type {targetType} is not supported by FloatingPointModelBinder.");

            bindingContext.Result = ModelBindingResult.Success(parsedValue);
        }
        catch (Exception ex)
        {
            bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, ex.Message);
        }

        return Task.CompletedTask;
    }
}
