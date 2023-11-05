using Microsoft.AspNetCore.Mvc.Testing;
using Orleans.TestingHost;
using Xunit;

namespace ClientTest;

public class OrleansClusterFixture : IDisposable
{
    public TestCluster Cluster { get; }

    public OrleansClusterFixture()
    {
        var builder = new TestClusterBuilder(2);
        builder.Options.BaseSiloPort = new Random().Next(1025, ushort.MaxValue);
        Cluster = builder.Build();
        Cluster.Deploy();
    }

    public void Dispose()
    {
        Cluster.StopAllSilos();
    }
}

internal static class NotificationApiFactory
{
    internal static WebApplicationFactory<Program> GetApplication()
    {
        return new WebApplicationFactory<Program>();
    }

    internal static HttpClient GetClient(WebApplicationFactory<Program> webApp)
    {
        var client = webApp.CreateDefaultClient();
        return client;
    }
}

public class UnitTest1 : IClassFixture<OrleansClusterFixture>
{
    [Fact]
    public void Test1()
    {
        var app = NotificationApiFactory.GetApplication();
        NotificationApiFactory.GetClient(app);
    }
}