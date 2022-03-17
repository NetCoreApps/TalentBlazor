using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using Bogus;
using System.Diagnostics;

[assembly: HostingStartup(typeof(TalentBlazor.ConfigureDb))]

namespace TalentBlazor;

public class ConfigureDb : IHostingStartup
{
    public void Configure(IWebHostBuilder builder) => builder
        .ConfigureServices((context, services) =>
            services.AddSingleton<IDbConnectionFactory>(new OrmLiteConnectionFactory(
                ":memory:",
                SqliteDialect.Provider)))
        .ConfigureAppHost(appHost =>
        {
            Debug.WriteLine("Faker seed: " + Randomizer.Seed);

            using var db = appHost.Resolve<IDbConnectionFactory>().Open();

            var profilesDir = appHost.IsDevelopmentEnvironment()
                ? "~/wwwroot/profiles".MapProjectPath()
                : Path.Join(AppContext.BaseDirectory, "wwwroot", "profiles");
            db.SeedTalent(profilesDir: profilesDir);
            db.SeedAttachments(appHost, appHost.ContentRootDirectory.RealPath.CombineWith("App_Data"));
        });
}