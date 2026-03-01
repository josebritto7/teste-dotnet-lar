using Lar.TesteDotNet.Application.Handlers.Dtos;
using Lar.TesteDotNet.Application.Handlers.Pessoas.Commands.Activate;
using Lar.TesteDotNet.Application.Handlers.Pessoas.Commands.Create;
using Lar.TesteDotNet.Application.Handlers.Pessoas.Commands.Deactivate;
using Lar.TesteDotNet.Application.Handlers.Pessoas.Commands.Update;
using Lar.TesteDotNet.Application.Handlers.Pessoas.Queries.Get;
using Lar.TesteDotNet.Application.Handlers.Pessoas.Queries.GetById;
using Lar.TesteDotNet.Application.Pagination;
using Lar.TesteDotNet.Shared.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace Lar.TesteDotNet.Presentation.Controllers;

public class PessoasController : BaseController
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedList<PessoaDto>>>> GetAll(
        int limit,
        string? cursor,
        CancellationToken ct)
    {
        var result = await Mediator.SendQueryAsync<GetPessoasQuery, RequestResult<PagedList<PessoaDto>>>(
            new GetPessoasQuery { Limit = limit, Cursor = cursor }, ct);

        return FromResult(result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<ApiResponse<PessoaDto>>> GetById(long id, CancellationToken ct)
    {
        var result =
            await Mediator.SendQueryAsync<GetPessoaByIdQuery, RequestResult<PessoaDto>>(
                new GetPessoaByIdQuery(id), ct);
        return FromResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<long>>> Create([FromBody] CreatePessoaCommand command,
        CancellationToken ct)
    {
        var result = await Mediator.SendCommandAsync<CreatePessoaCommand, RequestResult<long>>(command, ct);
        return CreatedFromResult(result, nameof(GetById), id => new { id }, "Pessoa criada com sucesso");
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<ApiResponse<string>>> Update(long id, [FromBody] UpdatePessoaCommand command,
        CancellationToken ct)
    {
        command.Id = id;
        var result = await Mediator.SendCommandAsync<UpdatePessoaCommand, RequestResult>(command, ct);
        return FromResult(result, successMessage: "Pessoa atualizada com sucesso");
    }

    [HttpPatch("{id:long}/ativar")]
    public async Task<ActionResult<ApiResponse<string>>> Activate(long id, CancellationToken ct)
    {
        var result =
            await Mediator.SendCommandAsync<ActivatePessoaCommand, RequestResult>(
                new ActivatePessoaCommand { Id = id }, ct);
        return FromResult(result, successMessage: "Pessoa ativada com sucesso");
    }

    [HttpPatch("{id:long}/desativar")]
    public async Task<ActionResult<ApiResponse<string>>> Deactivate(long id, CancellationToken ct)
    {
        var result =
            await Mediator.SendCommandAsync<DeactivatePessoaCommand, RequestResult>(
                new DeactivatePessoaCommand { Id = id }, ct);
        return FromResult(result, successMessage: "Pessoa desativada com sucesso");
    }
}
