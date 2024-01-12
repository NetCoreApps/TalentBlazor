using ServiceStack.Auth;
using TalentBlazor.Data;

[assembly: HostingStartup(typeof(TalentBlazor.ConfigureAuth))]

namespace TalentBlazor;

public class ConfigureAuth : IHostingStartup
{
    public void Configure(IWebHostBuilder builder) => builder
        .ConfigureServices(services =>
        {
            services.AddPlugin(new AuthFeature(IdentityAuth.For<ApplicationUser>(options => {
                options.EnableCredentialsAuth = true;
                options.SessionFactory = () => new CustomUserSession();
            })));
        });
}
