using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Diagnostics;
using Lar.TesteDotNet.Shared.Wrappers;

namespace Lar.TesteDotNet.Presentation.Infrastructure;

internal sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Unhandled exception occurred");

        int statusCode;
        object body;

        switch (exception)
        {
            case ValidationException ve:
                statusCode = StatusCodes.Status400BadRequest;
                body = ApiResponse<object>.Fail(ve.Message);
                break;
            case InvalidOperationException ioe:
                statusCode = StatusCodes.Status400BadRequest;
                body = ApiResponse<object>.Fail(ioe.Message);
                break;
            case FluentValidation.ValidationException fve:
                statusCode = StatusCodes.Status400BadRequest;
                var errors = fve.Errors
                    .Select(e => string.IsNullOrWhiteSpace(e.PropertyName)
                        ? e.ErrorMessage
                        : $"{e.PropertyName}: {e.ErrorMessage}")
                    .ToArray();
                body = ApiResponse<object>.Fail(errors);
                break;
            default:
                statusCode = StatusCodes.Status500InternalServerError;
                body = ApiResponse<object>.Fail("Ocorreu um erro inesperado. Tente novamente mais tarde.");
                break;
        }

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(body, cancellationToken);
        return true;
    }
}