using Microsoft.AspNetCore.Mvc;
using Lar.TesteDotNet.Shared.Interfaces.Messaging;
using Lar.TesteDotNet.Shared.Wrappers;

namespace Lar.TesteDotNet.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[ProducesResponseType(typeof(ApiResponse<object>), 400)]
[ProducesResponseType(typeof(ApiResponse<object>), 404)]
[ProducesResponseType(typeof(ApiResponse<object>), 409)]
[ProducesResponseType(typeof(ApiResponse<object>), 500)]
[ProducesResponseType(typeof(ApiResponse<object>), 200)]
public class BaseController : ControllerBase
{
    private IMediator? _mediator;

    protected IMediator Mediator => _mediator ??=
        (IMediator)HttpContext.RequestServices.GetRequiredService(typeof(IMediator))
        ?? throw new InvalidOperationException("IMediator não está registrado no container de DI.");

    protected ActionResult<ApiResponse<T>> FromResult<T>(RequestResult<T> requestResult,
        int successStatus = StatusCodes.Status200OK)
    {
        if (requestResult.IsFailure)
        {
            var status = requestResult.Error?.Status ?? StatusCodes.Status400BadRequest;
            return StatusCode(status, ApiResponse<T>.Fail(requestResult.Error?.Message ?? "Operação falhou"));
        }

        var payload = ApiResponse<T>.Ok(requestResult.Value!);
        return successStatus == StatusCodes.Status200OK
            ? Ok(payload)
            : StatusCode(successStatus, payload);
    }

    protected ActionResult<ApiResponse<string>> FromResult(RequestResult requestResult,
        int successStatus = StatusCodes.Status200OK, string successMessage = "Operação realizada com sucesso")
    {
        if (requestResult.IsFailure)
        {
            var status = requestResult.Error?.Status ?? StatusCodes.Status400BadRequest;
            return StatusCode(status, ApiResponse<string>.Fail(requestResult.Error?.Message ?? "Operação falhou"));
        }

        var payload = ApiResponse<string>.Ok(successMessage);
        return successStatus == StatusCodes.Status200OK
            ? Ok(payload)
            : StatusCode(successStatus, payload);
    }

    protected ActionResult<ApiResponse<TId>> CreatedFromResult<TId>(RequestResult<TId> result, string actionName,
        Func<TId, object> routeValuesFactory, string successMessage = "Criado com sucesso")
    {
        if (result.IsFailure)
        {
            var status = result.Error?.Status ?? StatusCodes.Status400BadRequest;
            return StatusCode(status, ApiResponse<TId>.Fail(result.Error?.Message ?? "Falha ao criar recurso"));
        }

        var id = result.Value!;
        var routeValues = routeValuesFactory(id);
        return CreatedAtAction(actionName, routeValues, ApiResponse<TId>.Created(id, successMessage));
    }
}