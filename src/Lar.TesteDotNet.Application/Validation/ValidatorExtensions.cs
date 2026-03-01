using FluentValidation;
using Lar.TesteDotNet.Domain.Common;
using Lar.TesteDotNet.Shared.Interfaces.Database;

namespace Lar.TesteDotNet.Application.Validation;

public static class ValidatorExtensions
{
    public static IRuleBuilderOptions<T, long> Exists<T, TEntity>(
        this IRuleBuilder<T, long> ruleBuilder,
        IRepository<TEntity> repository) where TEntity : Entity
    {
        Func<long, bool> existsValidation = idValue => { return repository.Exists(b => b.Id == idValue); };

        return ruleBuilder.Must(existsValidation);
    }
}