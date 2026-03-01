using Lar.TesteDotNet.Application.Handlers.Pessoas.Commands.Create;
using Lar.TesteDotNet.Domain.Common;
using Lar.TesteDotNet.Infrastructure.Database;
using Lar.TesteDotNet.Presentation;
using NetArchTest.Rules;

namespace Lar.TesteDotNet.Tests.Integration.Architecture;

[Category("Architecture")]
public class ArchitectureTests
{
    [Test]
    public void Domain_Should_Not_Depend_On_Application_Infrastructure_Api()
    {
        var result = Types.InAssembly(typeof(DomainResult).Assembly)
            .Should().NotHaveDependencyOnAny(
                typeof(CreatePessoaCommand).Namespace!,
                typeof(AppDbContext).Namespace!,
                typeof(Program).Namespace!
            ).GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Test]
    public void Application_Should_Not_Depend_On_Infrastructure_Or_Api()
    {
        var result = Types.InAssembly(typeof(CreatePessoaCommand).Assembly)
            .Should().NotHaveDependencyOnAny(
                typeof(AppDbContext).Namespace!,
                typeof(Program).Namespace!
            ).GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Test]
    public void Infrastructure_Should_Not_Depend_On_Api()
    {
        var result = Types.InAssembly(typeof(AppDbContext).Assembly)
            .Should().NotHaveDependencyOnAny(typeof(Program).Namespace!)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}
