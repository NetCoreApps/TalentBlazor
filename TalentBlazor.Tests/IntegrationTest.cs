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
        ServiceStack.Licensing.RegisterLicense("OSS GPL-3.0 2024 https://github.com/NetCoreApps/TalentBlazor Ed7tyPlTANoOB8+YQd+CG4To5kRnOtuV9AktLY3vdvx0fEn+gcWfDy8lPldYkBoiLXbAmrcfQQbysHeuz1C/VBBLE1r1/UVblt6swhFjjbCBhl/XvcjIxEt2vigmXiuNGEkreqFQHEh4ttyv3l2Jb6jm0iGE/dXgGEKIkj9obuc=");
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
