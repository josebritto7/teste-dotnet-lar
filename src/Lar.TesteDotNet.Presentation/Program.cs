using Lar.TesteDotNet.Presentation.Configurations;
using Lar.TesteDotNet.Presentation.Configurations.Database;
using Lar.TesteDotNet.Presentation.Configurations.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Migrate();
app.Seed(builder.Configuration);

app.Run();

namespace Lar.TesteDotNet.Presentation
{
    public partial class Program
    {
    
    }
}
