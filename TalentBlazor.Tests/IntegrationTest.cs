using Funq;
using ServiceStack;
using NUnit.Framework;
using TalentBlazor.ServiceInterface;
using TalentBlazor.ServiceModel;

namespace TalentBlazor.Tests;

public class IntegrationTest
{
    const string BaseUri = "http://localhost:2000/";
    private readonly ServiceStackHost appHost;

    class AppHost : AppSelfHostBase
    {
        public AppHost() : base(nameof(IntegrationTest), typeof(MyServices).Assembly) { }

        public override void Configure(Container container)
        {
        }
    }

    public IntegrationTest()
    {
        Licensing.RegisterLicense("OSS MIT 2023 https://github.com/NetCoreApps/TalentBlazorServer NdCmSFOfRpijw7CNjO4WXv4gYzeHjYSOaFRVlbp5rezlsB1FtDHUA34hYsJDMbxWDMiXqG29DPzPvJb6oXqLqkdCISRKOhhjZKb1KleG67PaHj9B2E/s65m9mB7Okq3uUHJzxpkn/3M0lvI4qL6Gb6jfmrx0+N0QxYsY41rjQpo=");
        appHost = new AppHost()
            .Init()
            .Start(BaseUri);
    }

    [OneTimeTearDown]
    public void OneTimeTearDown() => appHost.Dispose();

    public IServiceClient CreateClient() => new JsonServiceClient(BaseUri);

    [Test]
    public void Can_call_Hello_Service()
    {
        var client = CreateClient();

        var response = client.Get(new Hello { Name = "World" });

        Assert.That(response.Result, Is.EqualTo("Hello, World!"));
    }
}