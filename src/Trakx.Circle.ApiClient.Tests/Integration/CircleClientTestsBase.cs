using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Trakx.Utils.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Trakx.Circle.ApiClient.Tests.Integration;

[Collection(nameof(ApiTestCollection))]
public class CircleClientTestsBase
{
    protected readonly ServiceProvider ServiceProvider;
    protected ILogger Logger;

    public CircleClientTestsBase(CircleApiFixture apiFixture, ITestOutputHelper output)
    {
        Logger = new LoggerConfiguration().WriteTo.TestOutput(output).CreateLogger();

        ServiceProvider = apiFixture.ServiceProvider;
    }
}

[CollectionDefinition(nameof(ApiTestCollection))]
public class ApiTestCollection : ICollectionFixture<CircleApiFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}

public class CircleApiFixture : IDisposable
{
    public ServiceProvider ServiceProvider { get; }

    public CircleApiFixture()
    {
        var configuration = ConfigurationHelper.GetConfigurationFromEnv<CircleApiConfiguration>()
            with {
                BaseUrl = "https://api-sandbox.circle.com"
            };

        var serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton(configuration);
        serviceCollection.AddCircleClient(configuration);

        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing) return;
        ServiceProvider.Dispose();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
