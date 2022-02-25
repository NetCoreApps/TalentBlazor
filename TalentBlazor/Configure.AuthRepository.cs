using ServiceStack;
using ServiceStack.Web;
using ServiceStack.Data;
using ServiceStack.Html;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using TalentBlazor.Client;
using Bogus;
using ServiceStack.DataAnnotations;
using TalentBlazor.ServiceModel;
using ServiceStack.OrmLite;

[assembly: HostingStartup(typeof(TalentBlazor.ConfigureAuthRepository))]

namespace TalentBlazor;


// Custom User Table with extended Metadata properties
public class AppUser : UserAuth
{
    public string Title { get; set; }
    public string JobArea { get; set; }
    public string Location { get; set; }
    public string Team { get; set; }
    public int Salary { get; set; }
    public string About { get; set; }

    public Department Department { get; set; }
    public string? ProfileUrl { get; set; }
    public string? LastLoginIp { get; set; }

    public bool IsArchived { get; set; }
    public DateTime? ArchivedDate { get; set; }

    public DateTime? LastLoginDate { get; set; }
}



public class AppUserAuthEvents : AuthEvents
{
    public override async Task OnAuthenticatedAsync(IRequest httpReq, IAuthSession session, IServiceBase authService,
        IAuthTokens tokens, Dictionary<string, string> authInfo, CancellationToken token = default)
    {
        var authRepo = HostContext.AppHost.GetAuthRepositoryAsync(httpReq);
        using (authRepo as IDisposable)
        {
            var userAuth = (AppUser)await authRepo.GetUserAuthAsync(session.UserAuthId, token);
            userAuth.ProfileUrl = session.GetProfileUrl();
            userAuth.LastLoginIp = httpReq.UserHostAddress;
            userAuth.LastLoginDate = DateTime.UtcNow;
            await authRepo.SaveUserAuthAsync(userAuth, token);
        }
    }
}

public class ConfigureAuthRepository : IHostingStartup
{
    public void Configure(IWebHostBuilder builder) => builder
        .ConfigureServices(services => services.AddSingleton<IAuthRepository>(c =>
            new OrmLiteAuthRepository<AppUser, UserAuthDetails>(c.Resolve<IDbConnectionFactory>()) {
                UseDistinctRoleTables = true
            }))
        .ConfigureAppHost(appHost => {
            var authRepo = appHost.Resolve<IAuthRepository>();
            authRepo.InitSchema();
            using var db = appHost.Resolve<IDbConnectionFactory>().OpenDbConnection();
            CreateUser(authRepo, "admin@email.com", "Admin User", "p@55wOrd", roles: new[] { RoleNames.Admin });
            CreateUser(authRepo, "manager@email.com", "The Manager", "p@55wOrd", roles: new[] { AppRoles.Employee, AppRoles.Manager });
            CreateUser(authRepo, "employee@email.com", "A Employee", "p@55wOrd", roles: new[] { AppRoles.Employee });
            CreateUser(authRepo, "employee1@email.com", "A Employee 1", "p@55wOrd", roles: new[] { AppRoles.Employee });
            CreateUser(authRepo, "employee2@email.com", "A Employee 2", "p@55wOrd", roles: new[] { AppRoles.Employee });

            // Removing unused UserName in Admin Users UI 
            appHost.Plugins.Add(new ServiceStack.Admin.AdminUsersFeature {
                
                // Show custom fields in Search Results
                QueryUserAuthProperties = new() {
                    nameof(AppUser.Id),
                    nameof(AppUser.Email),
                    nameof(AppUser.DisplayName),
                    nameof(AppUser.Department),
                    nameof(AppUser.CreatedDate),
                    nameof(AppUser.LastLoginDate),
                },

                QueryMediaRules = new()
                {
                    MediaRules.ExtraSmall.Show<AppUser>(x => new { x.Id, x.Email, x.DisplayName }),
                    MediaRules.Small.Show<AppUser>(x => x.Department),
                },

                // Add Custom Fields to Create/Edit User Forms
                UserFormLayout = new() {
                    new()
                    {
                        Input.For<AppUser>(x => x.Email),
                    },
                    new()
                    {
                        Input.For<AppUser>(x => x.DisplayName),
                    },
                    new()
                    {
                        Input.For<AppUser>(x => x.Company),
                        Input.For<AppUser>(x => x.Department),
                    },
                    new() {
                        Input.For<AppUser>(x => x.PhoneNumber, c => c.Type = Input.Types.Tel)
                    },
                    new() {
                        Input.For<AppUser>(x => x.Nickname, c => {
                            c.Help = "Public alias (3-12 lower alpha numeric chars)";
                            c.Pattern = "^[a-z][a-z0-9_.-]{3,12}$";
                            //c.Required = true;
                        })
                    },
                    new() {
                        Input.For<AppUser>(x => x.ProfileUrl, c => c.Type = Input.Types.Url)
                    },
                    new() {
                        Input.For<AppUser>(x => x.IsArchived), Input.For<AppUser>(x => x.ArchivedDate),
                    },
                }
            });

        },
        afterConfigure: appHost => {
            appHost.AssertPlugin<AuthFeature>().AuthEvents.Add(new AppUserAuthEvents());
        });


    private static Faker<AppUser> appUserFaker = new Faker<AppUser>()
        .RuleFor(a => a.About, (faker) => faker.Lorem.Paragraph())
        .RuleFor(a => a.Department, (faker) => faker.Random.Enum<Department>())
        .RuleFor(a => a.BirthDate, (faker) => faker.Date.Between(new DateTime(1990, 1, 1), new DateTime(1950, 1, 1)))
        .RuleFor(a => a.FirstName, (faker) => faker.Name.FirstName())
        .RuleFor(a => a.LastName, (faker) => faker.Name.LastName())
        .RuleFor(a => a.Title, (faker) => faker.Name.JobTitle())
        .RuleFor(a => a.JobArea, (faker) => faker.Name.JobArea())
        .RuleFor(a => a.Salary, (faker) => faker.Random.Int(90, 250) * 1000);
        

    // Add initial Users to the configured Auth Repository
    public void CreateUser(IAuthRepository authRepo, string email, string name, string password, string[] roles)
    {
        if (authRepo.GetUserAuthByUserName(email) == null)
        {
            var newUser = appUserFaker.Generate();
            newUser.Email = email;
            newUser.DisplayName = name;
            var user = authRepo.CreateUserAuth(newUser, password);
            authRepo.AssignRoles(user, roles);
        }
    }
}