using Funq;
using TalentBlazor.Data;
using TalentBlazor.ServiceInterface;
using ServiceStack;
using ServiceStack.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ServiceStack.Admin;
using ServiceStack.Configuration;
using ServiceStack.IO;
using ServiceStack.Validation;
using TalentBlazor.ServiceModel;

[assembly: HostingStartup(typeof(TalentBlazor.AppHost))]

namespace TalentBlazor;

public class AppHost : AppHostBase, IHostingStartup
{
    public void Configure(IWebHostBuilder builder) => builder
        .ConfigureServices((context, services) =>
        {
            // Configure ASP.NET Core IOC Dependencies
            services.AddPlugin(new CorsFeature(allowedHeaders: "Content-Type,Authorization",
                allowOriginWhitelist: new[]{
                    "http://localhost:5000",
                    "https://localhost:5001",
                }, allowCredentials: true));
        });

    public AppHost() : base("TalentBlazor", typeof(MyServices).Assembly) { }

    // Configure your AppHost with the necessary configuration and dependencies your App needs
    public override void Configure(Container container)
    {
        SetConfig(new HostConfig
        {
            AdminAuthSecret = "secretz",
        });

        var wwwrootVfs = GetVirtualFileSource<FileSystemVirtualFiles>();
        var appDataVfs = new FileSystemVirtualFiles(ContentRootDirectory.RealPath.CombineWith("App_Data").AssertDir());
        Instance.Plugins.Add(new FilesUploadFeature(
            new UploadLocation("profiles", wwwrootVfs, allowExtensions:FileExt.WebImages,
                resolvePath: ctx => $"/profiles/{ctx.FileName}"),
            new UploadLocation("users", wwwrootVfs, allowExtensions:FileExt.WebImages,
                resolvePath: ctx => $"/profiles/users/{ctx.UserAuthId}.{ctx.FileExtension}"),
            new UploadLocation("applications", appDataVfs, maxFileCount: 3, maxFileBytes: 10_000_000,
                resolvePath: ctx => ctx.GetLocationPath((ctx.Dto is CreateJobApplication create
                        ? $"jobapp/{create.JobId}/{create.ContactId}/{ctx.FileName}"
                        : $"app/{ctx.Dto.GetId()}") + $"/{ctx.DateSegment}/{ctx.FileName}"),
                readAccessRole: RoleNames.AllowAnon, writeAccessRole: RoleNames.AllowAnon)
        ));
    }
}

public static class AppExtensions
{
    public static T DbExec<T>(this IServiceProvider services, Func<System.Data.IDbConnection, T> fn) =>
        services.DbContextExec<ApplicationDbContext, T>(ctx =>
        {
            ctx.Database.OpenConnection(); 
            return ctx.Database.GetDbConnection();
        }, fn);
}

// Add any additional metadata properties you want to store in the Users Typed Session
public class CustomUserSession : AuthUserSession
{
}