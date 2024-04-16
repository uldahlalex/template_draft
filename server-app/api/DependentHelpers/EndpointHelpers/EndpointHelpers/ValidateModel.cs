using System.ComponentModel.DataAnnotations;

namespace EndpointHelpers.EndpointHelpers;

public static class ValidateModelExtension
{
    public static void ValidateModel<T>(this T model)
    {
        var context = new ValidationContext(model, null, null);
        Validator.ValidateObject(model, context, true);
    }
}