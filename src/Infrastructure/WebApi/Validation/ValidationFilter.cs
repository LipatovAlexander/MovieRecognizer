using System.Reflection;
using FluentValidation;
using Infrastructure.WebApi.ApiResponses;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.WebApi.Validation;

[AttributeUsage(AttributeTargets.Parameter)]
public sealed class ValidateAttribute : Attribute;

public static class ValidationFilter
{
    public static EndpointFilterDelegate ValidationFilterFactory(
        EndpointFilterFactoryContext context,
        EndpointFilterDelegate next)
    {
        var validationDescriptors = GetValidators(context.MethodInfo, context.ApplicationServices);

        return validationDescriptors.Length != 0
            ? invocationContext => Validate(validationDescriptors, invocationContext, next)
            : next;
    }

    private static async ValueTask<object?> Validate(
        IEnumerable<ValidationDescriptor> validationDescriptors,
        EndpointFilterInvocationContext invocationContext,
        EndpointFilterDelegate next)
    {
        foreach (var descriptor in validationDescriptors)
        {
            var argument = invocationContext.Arguments[descriptor.ArgumentIndex];

            if (argument is null)
            {
                continue;
            }

            var validationContext = new ValidationContext<object>(argument);
            var validationResult = await descriptor.Validator.ValidateAsync(validationContext);

            if (validationResult.IsValid)
            {
                continue;
            }

            var details = validationResult.Errors.Select(e => e.ErrorMessage).ToArray();
            return Results.BadRequest(Responses.Error(CommonErrorCodes.InvalidRequest, details));
        }

        return await next.Invoke(invocationContext);
    }

    private static ValidationDescriptor[] GetValidators(MethodBase method, IServiceProvider serviceProvider)
    {
        var validators = new List<ValidationDescriptor>();

        var parameters = method.GetParameters();

        for (var i = 0; i < parameters.Length; i++)
        {
            var parameter = parameters[i];

            if (parameter.GetCustomAttribute<ValidateAttribute>() is null)
            {
                continue;
            }

            var validatorType = typeof(IValidator<>).MakeGenericType(parameter.ParameterType);

            if (serviceProvider.GetService(validatorType) is not IValidator validator)
            {
                continue;
            }

            var descriptor = new ValidationDescriptor
            {
                ArgumentIndex = i,
                Validator = validator
            };

            validators.Add(descriptor);
        }

        return validators.ToArray();
    }

    private sealed class ValidationDescriptor
    {
        public required int ArgumentIndex { get; init; }
        public required IValidator Validator { get; init; }
    }
}
