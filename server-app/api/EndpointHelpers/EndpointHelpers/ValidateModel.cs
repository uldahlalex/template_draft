using System.ComponentModel.DataAnnotations;

namespace api.EndpointHelpers.EndpointHelpers;

public static class ValidateModelExtension
{
    public static void ValidateModel<T>(this T model)
    {
        var context = new ValidationContext(model, serviceProvider: null, items: null);
        Validator.ValidateObject(model, context, true);
    }
}