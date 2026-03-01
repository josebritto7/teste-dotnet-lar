using System.Net;
using Bogus.Extensions.Brazil;
using Lar.TesteDotNet.Application.Handlers.Dtos;
using Lar.TesteDotNet.Application.Handlers.Pessoas.Commands.Create;
using Lar.TesteDotNet.Application.Handlers.Telefones.Commands.Create;
using Lar.TesteDotNet.Application.Handlers.Telefones.Commands.Update;
using Lar.TesteDotNet.Application.Pagination;
using Lar.TesteDotNet.Domain.Enums;
using Lar.TesteDotNet.Shared.Wrappers;
using Lar.TesteDotNet.Tests.Integration.Common;

namespace Lar.TesteDotNet.Tests.Integration.Api.Pessoas;

[Category("Api")]
public class TelefoneApiTests : IntegrationTestBase
{
    private long _pessoaId;
    private long _telefoneId;
    private static int _cpfSeed = 987654320;

    [OneTimeSetUp]
    public async Task SetupAsync()
    {
        var pessoaPayload = new CreatePessoaCommand
        {
            Nome = _faker.Name.FullName(),
            Cpf = NextCpf(),
            DataNascimento = _faker.Date.Past(28, DateTime.UtcNow.AddYears(-18))
        };

        var pessoaPost = await Client.PostAsJsonAsync("/api/pessoas", pessoaPayload);
        pessoaPost.StatusCode.Should().Be(HttpStatusCode.Created);
        var pessoaCreated = await pessoaPost.Content.ReadFromJsonAsync<ApiResponse<long>>();
        _pessoaId = pessoaCreated!.Data;

        var telefonePayload = new CreateTelefoneCommand
        {
            Tipo = TipoTelefone.Celular,
            Numero = RandomNumeroTelefone()
        };

        var telefonePost = await Client.PostAsJsonAsync($"/api/pessoas/{_pessoaId}/telefones", telefonePayload);
        telefonePost.StatusCode.Should().Be(HttpStatusCode.Created);
        var telefoneCreated = await telefonePost.Content.ReadFromJsonAsync<ApiResponse<long>>();
        _telefoneId = telefoneCreated!.Data;
    }

    [Test]
    public async Task Post_Then_GetById_Should_Work()
    {
        var payload = new CreateTelefoneCommand
        {
            Tipo = TipoTelefone.Celular,
            Numero = RandomNumeroTelefone()
        };

        var postResponse = await Client.PostAsJsonAsync($"/api/pessoas/{_pessoaId}/telefones", payload);
        postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var created = await postResponse.Content.ReadFromJsonAsync<ApiResponse<long>>();
        created.Should().NotBeNull();
        created!.Success.Should().BeTrue();
        created.Data.Should().BeGreaterThan(0);

        var getResponse = await Client.GetAsync($"/api/pessoas/{_pessoaId}/telefones/{created.Data}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await getResponse.Content.ReadFromJsonAsync<ApiResponse<TelefoneDto>>();
        result.Should().NotBeNull();
        result!.Success.Should().BeTrue();
        result.Data!.Id.Should().Be(created.Data);
        result.Data.PessoaId.Should().Be(_pessoaId);
        result.Data.Tipo.Should().Be(payload.Tipo);
        result.Data.Numero.Should().Be(payload.Numero);
    }

    [Test]
    public async Task Put_Update_Should_Work()
    {
        var update = new UpdateTelefoneCommand
        {
            Tipo = TipoTelefone.Comercial,
            Numero = RandomNumeroTelefone()
        };

        var put = await Client.PutAsJsonAsync($"/api/pessoas/{_pessoaId}/telefones/{_telefoneId}", update);
        put.StatusCode.Should().Be(HttpStatusCode.OK);
        var putResp = await put.Content.ReadFromJsonAsync<ApiResponse<string>>();
        putResp!.Success.Should().BeTrue();

        var get = await Client.GetAsync($"/api/pessoas/{_pessoaId}/telefones/{_telefoneId}");
        get.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await get.Content.ReadFromJsonAsync<ApiResponse<TelefoneDto>>();
        result!.Success.Should().BeTrue();
        result.Data!.Id.Should().Be(_telefoneId);
        result.Data.PessoaId.Should().Be(_pessoaId);
        result.Data.Tipo.Should().Be(update.Tipo);
        result.Data.Numero.Should().Be(update.Numero);
    }

    [Test]
    public async Task Delete_Should_Remove_Telefone()
    {
        var payload = new CreateTelefoneCommand
        {
            Tipo = TipoTelefone.Residencial,
            Numero = RandomNumeroTelefone()
        };

        var post = await Client.PostAsJsonAsync($"/api/pessoas/{_pessoaId}/telefones", payload);
        post.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await post.Content.ReadFromJsonAsync<ApiResponse<long>>();
        created.Should().NotBeNull();

        var delete = await Client.DeleteAsync($"/api/pessoas/{_pessoaId}/telefones/{created!.Data}");
        delete.StatusCode.Should().Be(HttpStatusCode.OK);
        var deleteResp = await delete.Content.ReadFromJsonAsync<ApiResponse<string>>();
        deleteResp!.Success.Should().BeTrue();

        var getDeleted = await Client.GetAsync($"/api/pessoas/{_pessoaId}/telefones/{created.Data}");
        getDeleted.StatusCode.Should().Be(HttpStatusCode.OK);
        var deletedResult = await getDeleted.Content.ReadFromJsonAsync<ApiResponse<TelefoneDto>>();
        deletedResult!.Success.Should().BeTrue();
        deletedResult.Data!.Id.Should().Be(0);
    }

    [Test]
    public async Task GetAll_With_Limit_And_Cursor_Should_Paginate()
    {
        for (var i = 0; i < 5; i++)
        {
            var payload = new CreateTelefoneCommand
            {
                Tipo = TipoTelefone.Celular,
                Numero = RandomNumeroTelefone()
            };
            var resp = await Client.PostAsJsonAsync($"/api/pessoas/{_pessoaId}/telefones", payload);
            resp.StatusCode.Should().Be(HttpStatusCode.Created);
            await Task.Delay(10);
        }

        var page1Resp = await Client.GetAsync($"/api/pessoas/{_pessoaId}/telefones?limit=2");
        page1Resp.StatusCode.Should().Be(HttpStatusCode.OK);
        var page1 = await page1Resp.Content.ReadFromJsonAsync<ApiResponse<PagedList<TelefoneDto>>>();
        page1!.Success.Should().BeTrue();
        page1.Data!.Items.Count.Should().Be(2);
        var cursor = page1.Data.Cursor;
        page1.Data.HasMore.Should().BeTrue();
        cursor.Should().NotBeNullOrEmpty();

        var page2Resp = await Client.GetAsync($"/api/pessoas/{_pessoaId}/telefones?limit=2&cursor={WebUtility.UrlEncode(cursor)}");
        page2Resp.StatusCode.Should().Be(HttpStatusCode.OK);
        var page2 = await page2Resp.Content.ReadFromJsonAsync<ApiResponse<PagedList<TelefoneDto>>>();
        page2!.Success.Should().BeTrue();
        page2.Data!.Items.Count.Should().Be(2);

        var page3Resp =
            await Client.GetAsync($"/api/pessoas/{_pessoaId}/telefones?limit=2&cursor={WebUtility.UrlEncode(page2.Data.Cursor)}");
        page3Resp.StatusCode.Should().Be(HttpStatusCode.OK);
        var page3 = await page3Resp.Content.ReadFromJsonAsync<ApiResponse<PagedList<TelefoneDto>>>();
        page3!.Success.Should().BeTrue();
        page3.Data!.Items.Count.Should().Be(2);
        page3.Data.HasMore.Should().BeFalse();
    }

    private string RandomNumeroTelefone()
    {
        return _faker.Phone.PhoneNumber("###########");
    }

    private static string NextCpf()
    {
        var baseDigits = Interlocked.Increment(ref _cpfSeed).ToString("000000000");

        var dv1 = CalculateDigit(baseDigits, 10);
        var dv2 = CalculateDigit(baseDigits + dv1, 11);

        return $"{baseDigits}{dv1}{dv2}";
    }

    private static int CalculateDigit(string source, int factor)
    {
        var sum = 0;
        for (var i = 0; i < source.Length; i++)
        {
            sum += (source[i] - '0') * factor;
            factor--;
        }

        var remainder = sum % 11;
        return remainder < 2 ? 0 : 11 - remainder;
    }
}
