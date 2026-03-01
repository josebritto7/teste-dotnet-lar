using System.Net;
using Bogus.Extensions.Brazil;
using Lar.TesteDotNet.Application.Handlers.Dtos;
using Lar.TesteDotNet.Application.Handlers.Pessoas.Commands.Create;
using Lar.TesteDotNet.Application.Handlers.Pessoas.Commands.Update;
using Lar.TesteDotNet.Application.Pagination;
using Lar.TesteDotNet.Shared.Wrappers;
using Lar.TesteDotNet.Tests.Integration.Common;

namespace Lar.TesteDotNet.Tests.Integration.Api.Pessoas;

[Category("Api")]
public class PessoaApiTests : IntegrationTestBase
{
    private long? _pessoaId;
    private static int _cpfSeed = 123456780;

    [OneTimeSetUp]
    public async Task SetupAsync()
    {
        var payload = BuildPessoaCreatePayload();

        var postResponse = await Client.PostAsJsonAsync("/api/pessoas", payload);
        await EnsureCreated(postResponse);

        var created = await postResponse.Content.ReadFromJsonAsync<ApiResponse<long>>();

        _pessoaId = created?.Data;
    }

    [Test]
    public async Task Post_Then_GetById_Should_Work()
    {
        var payload = BuildPessoaCreatePayload();

        var postResponse = await Client.PostAsJsonAsync("/api/pessoas", payload);
        await EnsureCreated(postResponse);

        var created = await postResponse.Content.ReadFromJsonAsync<ApiResponse<long>>();
        created.Should().NotBeNull();
        created!.Success.Should().BeTrue();
        created.Data.Should().BeGreaterThan(0);

        var getResponse = await Client.GetAsync($"/api/pessoas/{created.Data}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await getResponse.Content.ReadFromJsonAsync<ApiResponse<PessoaDto>>();
        result.Should().NotBeNull();
        result!.Success.Should().BeTrue();
        result.Data!.Id.Should().Be(created.Data);
        result.Data.Nome.Should().Be(payload.Nome);
        result.Data.Cpf.Should().Be(payload.Cpf);
        result.Data.DataNascimento.Should().Be(payload.DataNascimento.Date);
        result.Data.Ativo.Should().BeTrue();
    }

    [Test]
    public async Task Put_Update_Should_Work()
    {
        var id = _pessoaId;

        var update = new UpdatePessoaCommand
        {
            Nome = _faker.Name.FullName(),
            Cpf = _faker.Person.Cpf(false),
            DataNascimento = _faker.Date.Past(22, DateTime.UtcNow.AddYears(-18))
        };

        var put = await Client.PutAsJsonAsync($"/api/pessoas/{id}", update);
        put.StatusCode.Should().Be(HttpStatusCode.OK);
        var putResp = await put.Content.ReadFromJsonAsync<ApiResponse<string>>();
        putResp!.Success.Should().BeTrue();

        var get = await Client.GetAsync($"/api/pessoas/{id}");
        get.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await get.Content.ReadFromJsonAsync<ApiResponse<PessoaDto>>();
        result!.Success.Should().BeTrue();
        result.Data!.Id.Should().Be(id);
        result.Data.Nome.Should().Be(update.Nome);
        result.Data.Cpf.Should().Be(update.Cpf);
        result.Data.DataNascimento.Should().Be(update.DataNascimento.Date);
    }

    [Test]
    public async Task Post_Should_Return_BadRequest_When_Cpf_Duplicated()
    {
        var duplicateCpf = NextCpf();

        var firstPayload = new CreatePessoaCommand
        {
            Nome = _faker.Name.FullName(),
            Cpf = duplicateCpf,
            DataNascimento = _faker.Date.Past(30, DateTime.UtcNow.AddYears(-18))
        };

        var firstCreate = await Client.PostAsJsonAsync("/api/pessoas", firstPayload);
        await EnsureCreated(firstCreate);

        var secondPayload = new CreatePessoaCommand
        {
            Nome = _faker.Name.FullName(),
            Cpf = duplicateCpf,
            DataNascimento = _faker.Date.Past(30, DateTime.UtcNow.AddYears(-18))
        };

        var duplicateCreate = await Client.PostAsJsonAsync("/api/pessoas", secondPayload);
        duplicateCreate.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var response = await duplicateCreate.Content.ReadFromJsonAsync<ApiResponse<object>>();
        response.Should().NotBeNull();
        response!.Success.Should().BeFalse();
        response.Errors.Should().NotBeNull();
        response.Errors!.Any(e => e.Contains("Cpf", StringComparison.OrdinalIgnoreCase)
                                  && e.Contains("cadastrado", StringComparison.OrdinalIgnoreCase)).Should().BeTrue();
    }

    [Test]
    public async Task Deactivate_Then_Activate_Should_Toggle_Ativo()
    {
        var id = _pessoaId;

        var deact = await Client.PatchAsync($"/api/pessoas/{id}/desativar", null);
        deact.StatusCode.Should().Be(HttpStatusCode.OK);
        var deactResp = await deact.Content.ReadFromJsonAsync<ApiResponse<string>>();
        deactResp!.Success.Should().BeTrue();

        var getAfterDeact = await Client.GetAsync($"/api/pessoas/{id}");
        var afterDeact = await getAfterDeact.Content.ReadFromJsonAsync<ApiResponse<PessoaDto>>();
        afterDeact!.Data!.Ativo.Should().BeFalse();

        var act = await Client.PatchAsync($"/api/pessoas/{id}/ativar", null);
        act.StatusCode.Should().Be(HttpStatusCode.OK);
        var actResp = await act.Content.ReadFromJsonAsync<ApiResponse<string>>();
        actResp!.Success.Should().BeTrue();

        var getAfterAct = await Client.GetAsync($"/api/pessoas/{id}");
        var afterAct = await getAfterAct.Content.ReadFromJsonAsync<ApiResponse<PessoaDto>>();
        afterAct!.Data!.Ativo.Should().BeTrue();
    }

    [Test]
    public async Task GetAll_With_Limit_And_Cursor_Should_Paginate()
    {
        for (var i = 0; i < 5; i++)
        {
            var payload = BuildPessoaCreatePayload();
            var resp = await Client.PostAsJsonAsync("/api/pessoas", payload);
            await EnsureCreated(resp);
            await Task.Delay(10);
        }

        var page1Resp = await Client.GetAsync("/api/pessoas?limit=2");
        page1Resp.StatusCode.Should().Be(HttpStatusCode.OK);
        var page1 = await page1Resp.Content.ReadFromJsonAsync<ApiResponse<PagedList<PessoaDto>>>();
        page1!.Success.Should().BeTrue();
        page1.Data!.Items.Count.Should().Be(2);
        var cursor = page1.Data.Cursor;
        page1.Data.HasMore.Should().BeTrue();
        cursor.Should().NotBeNullOrEmpty();

        var page2Resp = await Client.GetAsync($"/api/pessoas?limit=2&cursor={WebUtility.UrlEncode(cursor)}");
        page2Resp.StatusCode.Should().Be(HttpStatusCode.OK);
        var page2 = await page2Resp.Content.ReadFromJsonAsync<ApiResponse<PagedList<PessoaDto>>>();
        page2!.Success.Should().BeTrue();
        page2.Data!.Items.Count.Should().Be(2);

        var page3Resp =
            await Client.GetAsync($"/api/pessoas?limit=2&cursor={WebUtility.UrlEncode(page2.Data.Cursor)}");
        page3Resp.StatusCode.Should().Be(HttpStatusCode.OK);
        var page3 = await page3Resp.Content.ReadFromJsonAsync<ApiResponse<PagedList<PessoaDto>>>();
        page3!.Success.Should().BeTrue();
        page3.Data!.Items.Count.Should().Be(2);
        page3.Data.HasMore.Should().BeFalse();
    }

    private CreatePessoaCommand BuildPessoaCreatePayload()
    {
        return new CreatePessoaCommand
        {
            Nome = _faker.Name.FullName(),
            Cpf = NextCpf(),
            DataNascimento = _faker.Date.Past(30, DateTime.UtcNow.AddYears(-18))
        };
    }

    private static async Task EnsureCreated(HttpResponseMessage response)
    {
        if (response.StatusCode == HttpStatusCode.Created) return;

        var body = await response.Content.ReadAsStringAsync();
        Assert.Fail($"Status: {(int)response.StatusCode} - {response.StatusCode}. Body: {body}");
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
