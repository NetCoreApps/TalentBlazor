using ServiceStack.Auth;
using TalentBlazor.Data;
using TalentBlazor.ServiceModel;

[assembly: HostingStartup(typeof(TalentBlazor.ConfigureAuth))]

namespace TalentBlazor;

public class ConfigureAuth : IHostingStartup
{
    public void Configure(IWebHostBuilder builder) => builder
        .ConfigureAppHost(appHost => 
        {
            appHost.Plugins.Add(new AuthFeature(IdentityAuth.For<ApplicationUser>(options => {
                options.EnableCredentialsAuth = true;
            })));
        });
}
