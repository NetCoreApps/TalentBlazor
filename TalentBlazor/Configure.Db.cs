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
        .ConfigureServices((context, services) => services.AddSingleton<IDbConnectionFactory>(new OrmLiteConnectionFactory(
            context.Configuration.GetConnectionString("DefaultConnection") ?? ":memory:",
            SqliteDialect.Provider)))
        .ConfigureAppHost(appHost =>
        {
            var seed = 1807832753;
            Randomizer.Seed = new Random(seed);
            Debug.WriteLine("Faker seed: " + seed);

            using var db = appHost.Resolve<IDbConnectionFactory>().Open();

            var profilesDir = appHost.IsDevelopmentEnvironment()
                ? "~/wwwroot/profiles".MapProjectPath()
                : Path.Join(AppContext.BaseDirectory, "wwwroot", "profiles");
            db.SeedTalent(profilesDir: profilesDir);
            db.SeedAttachments(appHost, appHost.ContentRootDirectory.RealPath.CombineWith("App_Data"));
        });
}