using Meziantou.Extensions.Logging.Xunit;
using Meziantou.Extensions.Logging.Xunit.v3;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ITestOutputHelper = Xunit.ITestOutputHelper;

namespace EcommerceApp.Test.Abstractions;

public class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
{
    private readonly ITestOutputHelper _testOutputHelper;
    protected HttpClient HttpClient { get; init; }

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory, ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        var factoryWithLogger = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureLogging(x =>
            {
                x.ClearProviders();
                x.SetMinimumLevel(LogLevel.Debug);

                x.Services.AddSingleton<ILoggerProvider>(new XUnitLoggerProvider(_testOutputHelper));
            });
        });
        HttpClient = factoryWithLogger.CreateClient();
    }
}