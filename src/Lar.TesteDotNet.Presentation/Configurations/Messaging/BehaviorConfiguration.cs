using Microsoft.AspNetCore.Mvc;
using Lar.TesteDotNet.Shared.Wrappers;

namespace Lar.TesteDotNet.Presentation.Configurations.Messaging;

public static class BehaviorConfiguration
{
    public static IServiceCollection ConfigureBehavior(
        this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Where(kvp => kvp.Value?.Errors.Count > 0)
                    .SelectMany(kvp => kvp.Value!.Errors.Select(e => $"{kvp.Key}: {e.ErrorMessage}"))
                    .ToArray();

                var response = ApiResponse<object>.Fail(errors.Length > 0 ? errors : new[] { "Requisição inválida" });
                return new BadRequestObjectResult(response);
            };
        });

        return services;
    }
}