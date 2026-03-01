using System.Numerics;
using Lar.TesteDotNet.Domain.Errors;

namespace Lar.TesteDotNet.Domain.Common;

public static class Validator
{
    /// <summary>
    ///     Validação de propriedade nula.
    /// </summary>
    /// <param name="errors">Extensão dos erros.</param>
    /// <param name="value">Valor que será validado.</param>
    /// <param name="property">Nome da propriedade que está sendo validada.</param>
    /// <param name="entity">Nome da entidade que está sendo validada.</param>
    /// <param name="message">Mensagem que será lançada na exception.</param>
    /// <returns></returns>
    public static List<Error> NotNull(
        this List<Error> errors,
        object? value,
        string property,
        string entity,
        string? message = null)
    {
        if (value is not null) return errors;

        message ??= $"É obrigatório informar a propriedade {property}.";

        errors.Add(
            Error.Validation(
                $"{entity}.{property}.not_null",
                message));

        return errors;
    }

    /// <summary>
    ///     Validação de tamanho de propriedade.
    /// </summary>
    /// <param name="errors">Extensão dos erros.</param>
    /// <param name="value">Valor que será validado.</param>
    /// <param name="property">Nome da propriedade validada.</param>
    /// <param name="entity">Nome da entidade validade.</param>
    /// <param name="minLength">Tamanho mínimo da propriedade.</param>
    /// <param name="maxLength">Tamanho máximo da propriedade.</param>
    /// <param name="message">Mensagem que será lançada na exception.</param>
    /// <returns></returns>
    public static List<Error> Length(
        this List<Error> errors,
        string? value,
        string property,
        string entity,
        int minLength,
        int maxLength,
        string? message = null)
    {
        if (value is null) return errors;

        if (value.Length >= minLength && value.Length <= maxLength) return errors;

        message ??= minLength == maxLength
            ? $"Propriedade {property} com tamanho inválido, esperado {minLength}, tamanho atual {value.Length}."
            : $"Propriedade {property} com tamanho inválido, esperado entre {minLength} e {maxLength}, tamanho atual {value.Length}.";

        errors.Add(
            Error.Validation(
                $"{entity}.{property}.invalid_length",
                message));

        return errors;
    }

    /// <summary>
    ///     Validação genérica para bool
    /// </summary>
    /// <param name="errors">Extensão dos erros.</param>
    /// <param name="value">Valor que será validado.</param>
    /// <param name="property">Propriedade que será validada.</param>
    /// <param name="entity">Entidade que será validada.</param>
    /// <param name="message">Mensagem que será lançada na exception.</param>
    /// <param name="errCode">Codigo do erro lançado.</param>
    /// <returns></returns>
    public static List<Error> BeTrue(
        this List<Error> errors,
        bool value,
        string property,
        string entity,
        string message,
        string errCode)
    {
        if (value) return errors;

        errors.Add(
            Error.Validation(
                $"{entity}.{property}.{errCode}",
                message));

        return errors;
    }

    /// <summary>
    ///     Adição de erros existentes na lista
    /// </summary>
    /// <param name="errors">Extensão dos erros.</param>
    /// <param name="appendErrors">Erros que serão adicionados.</param>
    /// <returns></returns>
    public static List<Error> AddErrors(
        this List<Error> errors,
        List<Error>? appendErrors)
    {
        if (appendErrors is null) return errors;

        errors.AddRange(appendErrors);

        return errors;
    }

    /// <summary>
    ///     Comparação de valores para numéricos
    /// </summary>
    /// <param name="errors">Extensão dos erros.</param>
    /// <param name="value">Valor que será validado.</param>
    /// <param name="property">Propriedade que será validada.</param>
    /// <param name="entity">Entidade que será validada.</param>
    /// <param name="comparer">Valor que será usado para comparação.</param>
    /// <param name="message">Mensagem que será lançada na exception.</param>
    /// <typeparam name="T">Tipo que será comparado, obrigatório ser número.</typeparam>
    /// <returns></returns>
    public static List<Error> GreaterThan<T>(
        this List<Error> errors,
        T value,
        string property,
        string entity,
        T comparer,
        string? message = null) where T : INumber<T>
    {
        if (value > comparer) return errors;

        message ??= $"Propriedade {property} com valor inválido, deve ser maior do que {comparer}.";

        errors.Add(
            Error.Validation(
                $"{entity}.{property}.invalid_value",
                message));

        return errors;
    }

    /// <summary>
    ///     Executa as validações apenas quando a lista atual de erros for vazia.
    /// </summary>
    /// <param name="errors">Extensão dos erros.</param>
    /// <param name="action">Ação contendo o list dos erros</param>
    /// <returns></returns>
    public static List<Error> JoinWhenEmpty(
        this List<Error> errors,
        Action<List<Error>> action)
    {
        if (errors.Any()) return errors;

        action(errors);

        return errors;
    }

    
    /// <summary>
    ///     Executa as validações apenas quando a condição for True.
    /// </summary>
    /// <param name="errors">Extensão dos erros.</param>
    /// <param name="condition">Condição para executar as validações.</param>
    /// <param name="action">Ação contendo o list dos erros</param>
    /// <returns></returns>
    public static List<Error> JoinWhen(
        this List<Error> errors,
        bool condition,
        Action<List<Error>> action)
    {
        if (!condition) return errors;

        action(errors);

        return errors;
    }

    /// <summary>
    ///     Realiza as validações básicas para um ID.
    /// </summary>
    /// <param name="errors">Extensão dos erros.</param>
    /// <param name="id">Id que será validado.</param>
    /// <param name="property">Nome da propriedade validada.</param>
    /// <param name="entity">Nome da entidade validade.</param>
    /// <param name="message">Mensagem que será lançada na exception.</param>
    /// <returns></returns>
    public static List<Error> Id(
        this List<Error> errors,
        long? id,
        string property,
        string entity,
        string? message = null)
    {
        if (id > 0) return errors;

        message ??= $"{property} inválido.";

        errors.Add(
            Error.Validation(
                $"{entity}.{property}.invalid_value",
                message));

        return errors;
    }
}