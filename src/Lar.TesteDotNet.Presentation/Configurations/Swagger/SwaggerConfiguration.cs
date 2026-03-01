using System.Reflection;
using Microsoft.OpenApi.Models;

namespace Lar.TesteDotNet.Presentation.Configurations.Swagger;

public static class SwaggerConfiguration
{
    public static IServiceCollection ConfigureSwagger(
        this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Lar.TesteDotNet API",
                Version = "v1",
                Description = "Documentacao da API de avaliacao .NET LAR."
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            if (File.Exists(xmlPath))
                options.IncludeXmlComments(xmlPath);
        });

        return services;
    }

    public static WebApplication UseSwaggerDocumentation(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Lar.TesteDotNet API v1");
            options.RoutePrefix = "swagger";
            options.DisplayRequestDuration();
        });

        return app;
    }
}
