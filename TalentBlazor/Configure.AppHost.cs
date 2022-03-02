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

    public void Configure(IWebHostBuilder builder)
    {
        Licensing.RegisterLicense("OSS BSD-3-Clause 2022 https://github.com/NetCoreApps/TalentBlazor eRijOv1P5vqep+GPt26wpgS7wvOYekpC7d5ipolpjPmMq/7A7LRKKLL3uTqQnQo1ySi/Ms2crO/rd2Fyeh3IohisA7K7xYiCtMry+DKa41EJ6dZUDIU3wR0/dpHqVKao3UpO8+dawYjv/ok37ENcLo/zDwub609UcR8/ytl+3aw=");
        builder
            .ConfigureServices((context, services) =>
                services.ConfigureNonBreakingSameSiteCookies(context.HostingEnvironment));
    }
}
