using Lar.TesteDotNet.Application.Handlers.Dtos;
using Lar.TesteDotNet.Application.Handlers.Telefones.Commands.Create;
using Lar.TesteDotNet.Application.Handlers.Telefones.Commands.Delete;
using Lar.TesteDotNet.Application.Handlers.Telefones.Commands.Update;
using Lar.TesteDotNet.Application.Handlers.Telefones.Queries.Get;
using Lar.TesteDotNet.Application.Handlers.Telefones.Queries.GetById;
using Lar.TesteDotNet.Application.Pagination;
using Lar.TesteDotNet.Shared.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace Lar.TesteDotNet.Presentation.Controllers;

public class TelefonesController : BaseController
{
    [HttpGet("/api/pessoas/{pessoaId:long}/telefones")]
    public async Task<ActionResult<ApiResponse<PagedList<TelefoneDto>>>> GetAll(
        long pessoaId,
        int limit,
        string? cursor,
        CancellationToken ct)
    {
        var result = await Mediator.SendQueryAsync<GetTelefonesQuery, RequestResult<PagedList<TelefoneDto>>>(
            new GetTelefonesQuery { PessoaId = pessoaId, Limit = limit, Cursor = cursor }, ct);

        return FromResult(result);
    }

    [HttpGet("/api/pessoas/{pessoaId:long}/telefones/{id:long}")]
    public async Task<ActionResult<ApiResponse<TelefoneDto>>> GetById(long pessoaId, long id, CancellationToken ct)
    {
        var result =
            await Mediator.SendQueryAsync<GetTelefoneByIdQuery, RequestResult<TelefoneDto>>(
                new GetTelefoneByIdQuery(pessoaId, id), ct);
        return FromResult(result);
    }

    [HttpPost("/api/pessoas/{pessoaId:long}/telefones")]
    public async Task<ActionResult<ApiResponse<long>>> Create(long pessoaId, [FromBody] CreateTelefoneCommand command,
        CancellationToken ct)
    {
        command.PessoaId = pessoaId;

        var result = await Mediator.SendCommandAsync<CreateTelefoneCommand, RequestResult<long>>(command, ct);
        return CreatedFromResult(
            result,
            nameof(GetById),
            id => new { pessoaId, id },
            "Telefone criado com sucesso");
    }

    [HttpPut("/api/pessoas/{pessoaId:long}/telefones/{id:long}")]
    public async Task<ActionResult<ApiResponse<string>>> Update(long pessoaId, long id,
        [FromBody] UpdateTelefoneCommand command,
        CancellationToken ct)
    {
        command.PessoaId = pessoaId;
        command.Id = id;

        var result = await Mediator.SendCommandAsync<UpdateTelefoneCommand, RequestResult>(command, ct);
        return FromResult(result, successMessage: "Telefone atualizado com sucesso");
    }

    [HttpDelete("/api/pessoas/{pessoaId:long}/telefones/{id:long}")]
    public async Task<ActionResult<ApiResponse<string>>> Delete(long pessoaId, long id, CancellationToken ct)
    {
        var result =
            await Mediator.SendCommandAsync<DeleteTelefoneCommand, RequestResult>(
                new DeleteTelefoneCommand { Id = id, PessoaId = pessoaId }, ct);

        return FromResult(result, successMessage: "Telefone removido com sucesso");
    }
}
