using FluentValidation;
using Lar.TesteDotNet.Application.Messaging;
using Lar.TesteDotNet.Application.Messaging.Behaviors;
using Lar.TesteDotNet.Shared.Interfaces.Messaging;

namespace Lar.TesteDotNet.Presentation.Configurations.Messaging;

public static class MediatorConfiguration
{
    public static IServiceCollection ConfigureMediator(
        this IServiceCollection services)
    {
        var applicationAssembly = typeof(Mediator).Assembly;
        services.AddValidatorsFromAssembly(applicationAssembly);
        
        services.Scan(scan => scan
            .FromAssemblies(applicationAssembly)
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Decorate(typeof(ICommandHandler<,>), typeof(ValidationCommandBehavior<,>));
        services.Decorate(typeof(IQueryHandler<,>), typeof(ValidationQueryBehavior<,>));

        services.AddScoped<IMediator, Mediator>();

        return services;
    }
}