using Funq;
using ServiceStack;
using TalentBlazor.ServiceInterface;

[assembly: HostingStartup(typeof(TalentBlazor.AppHost))]

namespace TalentBlazor;

public class AppHost : AppHostBase, IHostingStartup
{
    public AppHost() : base("TalentBlazor", typeof(MyServices).Assembly) { }

    public override void Configure(Container container)
    {
        SetConfig(new HostConfig {
        });

        Plugins.Add(new CorsFeature(allowedHeaders: "Content-Type,Authorization",
            allowOriginWhitelist: new[]{
            "http://localhost:5000",
            "https://localhost:5001",
            "https://" + Environment.GetEnvironmentVariable("DEPLOY_CDN")
        }, allowCredentials: true));
    }

    public void Configure(IWebHostBuilder builder) => builder
        .ConfigureServices((context, services) => 
            services.ConfigureNonBreakingSameSiteCookies(context.HostingEnvironment));
}
