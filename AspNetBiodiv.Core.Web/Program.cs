using AspNetBiodiv.Core.Web.Entities;
using AspNetBiodiv.Core.Web.Plumbing.CacheMonitoring;
using AspNetBiodiv.Core.Web.Plumbing.Middleware;
using AspNetBiodiv.Core.Web.Services;
using AspNetBiodiv.Core.Web.Services.Especes;
using AspNetBiodiv.Core.Web.Services.Observations;
using AspNetBiodiv.Core.Web.Services.Statistiques;
using AspNetBiodiv.Core.Web.Services.UserData;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using AspNetBiodiv.Core.Web.Services.Email;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// Add request logging
builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders;
});
builder.Services.AddW3CLogging(options =>
{
    options.LogDirectory = @"C:\temp\W3C_logs";
});

builder.Services.AddDbContext<EspecesContext>(options =>
{
    options
        .UseSqlServer(builder.Configuration.GetConnectionString("Especes"));
    //.EnableDetailedErrors();
});

if (builder.Configuration["USE_FAKES"] == "1")
{
    builder.Services.AddSingleton<ITaxonomie, BogusTaxonomie>();
    builder.Services.AddSingleton<IObservations, FakeObservations>();
}
else
{
    builder.Services.AddScoped<IImportService, ImportService>();
    builder.Services.AddScoped<ITaxonomie, DbTaxonomie>();
    builder.Services.AddScoped<IObservations, DbObservations>();
}

builder.Services.Configure<ConfirmationEmailOptions>(
    builder.Configuration.GetSection(nameof(ConfirmationEmailOptions)));

builder.Services.AddScoped<IEmailSender, FakeEmailSender>();

builder.Services
    .AddDefaultIdentity<BiodivUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<EspecesContext>();
builder.Services.AddScoped<IUserDataService, IdentityUserDataService>();
builder.Services.AddSingleton<ICacheMonitor, CacheMonitor>();
builder.Services.AddSingleton<ICommunes, StaticCommunes>();
builder.Services.AddSingleton<IStatisticsService, StatisticsService>();
builder.Services.AddSingleton<AcmeVersionMiddleware>(); 
builder.Services.AddRazorPages();
builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache();

var app = builder.Build();

app.UseAcmeVersion();

app.UseStatusCodePagesWithReExecute("/Error/Index/{0}");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    using var scope = app.Services.CreateScope();
    var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetService<UserManager<BiodivUser>>();

    if (!await roleManager.RoleExistsAsync(Roles.Administrator))
    {
        await roleManager.CreateAsync(new IdentityRole(Roles.Administrator));
    }

    if (await userManager.Users.CountAsync() == 1)
    {
        var user = userManager.Users.Single();
        await userManager.AddToRoleAsync(user, Roles.Administrator);
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.MapRazorPages();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
