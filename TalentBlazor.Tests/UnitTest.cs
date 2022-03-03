using NUnit.Framework;
using ServiceStack;
using ServiceStack.Testing;
using TalentBlazor.ServiceInterface;
using TalentBlazor.ServiceModel;

namespace TalentBlazor.Tests;

public class UnitTest
{
    private readonly ServiceStackHost appHost;

    public UnitTest()
    {
        appHost = new BasicAppHost().Init();
        appHost.Container.AddTransient<TalentServices>();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown() => appHost.Dispose();
}
