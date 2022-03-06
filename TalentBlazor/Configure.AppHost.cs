using Funq;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.IO;
using TalentBlazor.ServiceInterface;
using TalentBlazor.ServiceModel;

[assembly: HostingStartup(typeof(TalentBlazor.AppHost))]

namespace TalentBlazor;

public class AppHost : AppHostBase, IHostingStartup
{
    public AppHost() : base("TalentBlazor", typeof(TalentServices).Assembly) { }

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

        var wwwrootVfs = VirtualFileSources.GetFileSystemVirtualFiles();
        var appDataVfs = new FileSystemVirtualFiles(ContentRootDirectory.RealPath.CombineWith("App_Data").AssertDir());
        Plugins.Add(new FilesUploadFeature(
            new UploadLocation("profiles", wwwrootVfs, allowExtensions:FileExt.WebImages,
                resolvePath: (req, fileName) => $"/profiles/{fileName}"),
            new UploadLocation("users", wwwrootVfs, allowExtensions:FileExt.WebImages,
                resolvePath: (req, fileName) => $"/profiles/users/{req.GetSession().UserAuthId}.{fileName.LastRightPart('.')}"),
            new UploadLocation("applications", appDataVfs, maxFileCount: 3, maxFileBytes: 10_000_000,
                resolvePath: (req, fileName) => $"/uploads/applications/{((IHasJobId)req.Dto).JobId}/{DateTime.UtcNow:yyyy/MM/dd}/{fileName}",
                accessRole: RoleNames.AllowAnon)
        ));
    }

    public void Configure(IWebHostBuilder builder) => builder
        .ConfigureServices((context, services) => 
            services.ConfigureNonBreakingSameSiteCookies(context.HostingEnvironment));
}
