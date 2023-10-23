using ServiceStack;

ServiceStack.Licensing.RegisterLicense("OSS MIT 2023 https://github.com/NetCoreApps/TalentBlazor wbeIoAvm5/0HU2BbPyNKdoJehyNd5S+kvOkGT3UEnEON6MWBMq+9+IIvIuNz1337XFxtnsPjMsSWH2KVBjytfor09DDt9BVhD1nTYKZOQOy/XiTteuQX1TxEwDM8L5YWFBHNJK2+8OqtgEFG1zpHj2OeD+EFQ1wm7zNmI+P5XQU=");
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseHttpsRedirection();
}
app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseServiceStack(new AppHost());

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapControllers();
    endpoints.MapFallbackToFile("index.html");
});

app.Run();
