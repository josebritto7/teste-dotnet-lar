using Bogus;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Lar.TesteDotNet.Tests.Integration.Common;

public abstract class IntegrationTestBase : IDisposable
{
    protected readonly Faker _faker = new("pt_BR");
    protected readonly HttpClient Client;
    protected readonly CustomWebAppFactory Factory;
    protected readonly IServiceProvider Services;

    protected IntegrationTestBase()
    {
        Factory = new CustomWebAppFactory();
        Client = Factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        Services = Factory.Services;
    }

    public void Dispose()
    {
        try
        {
            Client.Dispose();
        }
        catch
        {
        }

        try
        {
            Factory.Dispose();
        }
        catch
        {
        }

        GC.SuppressFinalize(this);
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        Dispose();
    }
}