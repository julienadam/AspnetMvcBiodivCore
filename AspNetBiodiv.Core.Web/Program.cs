using AspNetBiodiv.Core.Web.Entities;
using AspNetBiodiv.Core.Web.Plumbing.CacheMonitoring;
using AspNetBiodiv.Core.Web.Plumbing.Middleware;
using AspNetBiodiv.Core.Web.Services;
using AspNetBiodiv.Core.Web.Services.Especes;
using AspNetBiodiv.Core.Web.Services.Observations;
using AspNetBiodiv.Core.Web.Services.Statistiques;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

if (builder.Configuration["USE_FAKES"] == "1")
{
    builder.Services.AddSingleton<ITaxonomie, BogusTaxonomie>();
    builder.Services.AddSingleton<IObservations, FakeObservations>();
}
else
{
    builder.Services.AddDbContext<EspecesContext>(options =>
    {
        options
            .UseSqlServer(builder.Configuration.GetConnectionString("Especes"))
            .EnableDetailedErrors();
    });


    builder.Services.AddScoped<ITaxonomie, DbTaxonomie>();
    builder.Services.AddScoped<IObservations, DbObservations>();
}

builder.Services.AddDefaultIdentity<BiodivUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<EspecesContext>();

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
