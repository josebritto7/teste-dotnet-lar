using AwesomeAssertions;
using Lar.TesteDotNet.Domain.Common;
using Lar.TesteDotNet.Domain.Errors;

namespace Lar.TesteDotNet.Tests.Domain.Common;

public class DomainResultTests
{
    [Test]
    public void DomainResultT_Ok_ShouldSetSuccessAndValue()
    {
        var res = DomainResult<int>.Ok(42);

        res.IsSuccess.Should().BeTrue();
        res.Value.Should().Be(42);
        res.Errors.Should().BeEmpty();
    }

    [Test]
    public void DomainResultT_Fail_ShouldSetErrors()
    {
        var errors = new List<Error> { Error.Validation("x", "y") };

        var res = DomainResult<int>.Fail(errors);

        res.IsSuccess.Should().BeFalse();
        res.Errors.Should().ContainSingle();
    }

    [Test]
    public void DomainResultT_From_ShouldReturnFail_WhenHasErrors()
    {
        var errors = new List<Error> { Error.Validation("x", "y") };
        var executed = false;

        var res = DomainResult<int>.From(() =>
        {
            executed = true;
            return 1;
        }, errors);

        res.IsSuccess.Should().BeFalse();
        executed.Should().BeFalse();
    }

    [Test]
    public void DomainResultT_From_ShouldExecuteFactory_WhenNoErrors()
    {
        var executed = false;

        var res = DomainResult<int>.From(() =>
        {
            executed = true;
            return 7;
        }, new List<Error>());

        res.IsSuccess.Should().BeTrue();
        res.Value.Should().Be(7);
        executed.Should().BeTrue();
    }

    [Test]
    public void DomainResult_Ok_ShouldSetSuccess()
    {
        var res = DomainResult.Ok();

        res.IsSuccess.Should().BeTrue();
        res.Errors.Should().BeEmpty();
    }

    [Test]
    public void DomainResult_Fail_ShouldSetErrors()
    {
        var res = DomainResult.Fail(new List<Error> { Error.Validation("a", "b") });

        res.IsSuccess.Should().BeFalse();
        res.Errors.Should().ContainSingle();
    }

    [Test]
    public void DomainResult_From_ShouldNotExecuteAction_WhenHasErrors()
    {
        var executed = false;

        var res = DomainResult.From(() => { executed = true; }, new List<Error> { Error.Validation("x", "y") });

        res.IsSuccess.Should().BeFalse();
        executed.Should().BeFalse();
    }

    [Test]
    public void DomainResult_From_ShouldExecuteAction_WhenNoErrors()
    {
        var executed = false;

        var res = DomainResult.From(() => { executed = true; }, new List<Error>());

        res.IsSuccess.Should().BeTrue();
        executed.Should().BeTrue();
    }
}