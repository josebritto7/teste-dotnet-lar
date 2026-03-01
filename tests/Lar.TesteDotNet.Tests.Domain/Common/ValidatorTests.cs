namespace Lar.TesteDotNet.Tests.Domain.Common;

public class ValidatorTests
{
    [Test]
    public void NotNull_ShouldAddError_WhenValueIsNull()
    {
        var errors = new List<Error>();

        errors.NotNull(null, "Name", "Pessoa");

        errors.Should().HaveCount(1);
        errors[0].Code.Should().Be("Pessoa.Name.not_null");
        errors[0].Message.Should().Contain("É obrigatório informar a propriedade Name.");
    }

    [Test]
    public void NotNull_ShouldNotAddError_WhenValueIsNotNull()
    {
        var errors = new List<Error>();

        errors.NotNull("john", "Name", "Pessoa");

        errors.Should().BeEmpty();
    }

    [Test]
    public void Length_ShouldAddError_WhenLengthOutsideRange()
    {
        var errors = new List<Error>();

        errors.Length("a", "Name", "Pessoa", 2, 5);

        errors.Should().HaveCount(1);
        errors[0].Code.Should().Be("Pessoa.Name.invalid_length");
        errors[0].Message.Should().Contain("esperado entre 2 e 5");
    }

    [Test]
    public void Length_ShouldNotAddError_WhenLengthWithinRange()
    {
        var errors = new List<Error>();

        errors.Length("john", "Name", "Pessoa", 2, 10);

        errors.Should().BeEmpty();
    }

    [Test]
    public void Length_ShouldNotAddError_WhenValueIsNull()
    {
        var errors = new List<Error>();

        errors.Length(null, "Name", "Pessoa", 2, 10);

        errors.Should().BeEmpty();
    }

    [Test]
    public void Length_ShouldUseSingleLengthMessage_WhenMinEqualsMax()
    {
        var errors = new List<Error>();

        errors.Length("abc", "Code", "Pessoa", 5, 5);

        errors.Should().HaveCount(1);
        errors[0].Message.Should().Contain("esperado 5");
    }

    [Test]
    public void BeTrue_ShouldAddError_WhenValueIsFalse()
    {
        var errors = new List<Error>();

        errors.BeTrue(false, "IsActive", "Pessoa", "Cliente já está inativo.", "invalid_operation");

        errors.Should().HaveCount(1);
        errors[0].Code.Should().Be("Pessoa.IsActive.invalid_operation");
        errors[0].Message.Should().Be("Cliente já está inativo.");
    }

    [Test]
    public void BeTrue_ShouldNotAddError_WhenValueIsTrue()
    {
        var errors = new List<Error>();

        errors.BeTrue(true, "IsActive", "Pessoa", "msg", "invalid_operation");

        errors.Should().BeEmpty();
    }

    [Test]
    public void AddErrors_ShouldAppend_WhenListHasErrors()
    {
        var errors = new List<Error>();
        var extra = new List<Error> { Error.Validation("e1", "m1"), Error.Validation("e2", "m2") };

        errors.AddErrors(extra);

        errors.Should().HaveCount(2);
        errors.Select(e => e.Code).Should().Contain(new[] { "e1", "e2" });
    }

    [Test]
    public void AddErrors_ShouldIgnore_WhenNull()
    {
        var errors = new List<Error>();

        errors.AddErrors(null);

        errors.Should().BeEmpty();
    }

    [Test]
    public void GreaterThan_ShouldNotAddError_WhenValueGreater()
    {
        var errors = new List<Error>();

        errors.GreaterThan(5, "Code", "Pessoa", 0);

        errors.Should().BeEmpty();
    }

    [Test]
    public void GreaterThan_ShouldAddError_WhenValueNotGreater()
    {
        var errors = new List<Error>();

        errors.GreaterThan(0, "Code", "Pessoa", 0);

        errors.Should().HaveCount(1);
        errors[0].Code.Should().Be("Pessoa.Code.invalid_value");
    }

    [Test]
    public void GreaterThan_ShouldWork_WithDecimal()
    {
        var errors = new List<Error>();

        errors.GreaterThan(1.0m, "Amount", "Invoice", 0.5m);

        errors.Should().BeEmpty();
    }

    [Test]
    public void JoinWhenEmpty_ShouldExecute_WhenNoErrors()
    {
        var errors = new List<Error>();

        errors.JoinWhenEmpty(e => e.AddErrors(new List<Error> { Error.Validation("x", "y") }));

        errors.Should().HaveCount(1);
    }

    [Test]
    public void JoinWhenEmpty_ShouldNotExecute_WhenHasErrors()
    {
        var errors = new List<Error> { Error.Validation("a", "b") };

        errors.JoinWhenEmpty(e => e.AddErrors(new List<Error> { Error.Validation("x", "y") }));

        errors.Should().HaveCount(1);
        errors[0].Code.Should().Be("a");
    }

    [Test]
    public void JoinWhen_ShouldExecute_WhenConditionTrue()
    {
        var errors = new List<Error>();

        errors.JoinWhen(true, e => e.AddErrors(new List<Error> { Error.Validation("x", "y") }));

        errors.Should().HaveCount(1);
    }

    [Test]
    public void JoinWhen_ShouldNotExecute_WhenConditionFalse()
    {
        var errors = new List<Error>();

        errors.JoinWhen(false, e => e.AddErrors(new List<Error> { Error.Validation("x", "y") }));

        errors.Should().BeEmpty();
    }
}