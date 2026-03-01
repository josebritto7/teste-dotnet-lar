using FluentValidation;
using Lar.TesteDotNet.Shared.Interfaces.Database;

namespace Lar.TesteDotNet.Application.Validation;

public class ValidatorBase<T> : AbstractValidator<T>
{
    protected ValidatorBase()
    {
    }

    protected ValidatorBase(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

    protected IUnitOfWork UnitOfWork { get; } = null!;
}