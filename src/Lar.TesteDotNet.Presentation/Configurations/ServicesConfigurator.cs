using Lar.TesteDotNet.Presentation.Configurations.Database;
using Lar.TesteDotNet.Presentation.Configurations.Mapping;
using Lar.TesteDotNet.Presentation.Configurations.Messaging;
using Lar.TesteDotNet.Presentation.Configurations.Swagger;
using Lar.TesteDotNet.Presentation.Infrastructure;

namespace Lar.TesteDotNet.Presentation.Configurations;

public static class ServicesConfigurator
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .ConfigureDatabase(configuration)
            .ConfigureMediator()
            .ConfigureBehavior()
            .ConfigureSwagger()
            .AddExceptionHandler<GlobalExceptionHandler>()
            .AddProblemDetails()
            .AddControllers();

        services.AddAuthorization();

        MappingConfiguration.ConfigureMapping();

        return services;
    }
}
