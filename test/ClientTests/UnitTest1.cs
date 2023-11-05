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

        // uses the same port/ids as the default LocalhostClustering
        // public static IClientBuilder UseLocalhostClustering(
        //     this IClientBuilder builder,
        //     int gatewayPort = 30000,
        //     string serviceId = ClusterOptions.DevelopmentServiceId,
        //     string clusterId = ClusterOptions.DevelopmentClusterId)
        builder.Options.BaseGatewayPort = 30000;
        builder.Options.ServiceId = "dev";
        builder.Options.ClusterId = "dev";

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