using AspNetBiodiv.Core.Web.Entities;
using AspNetBiodiv.Core.Web.Plumbing.Middleware;
using AspNetBiodiv.Core.Web.Services;
using AspNetBiodiv.Core.Web.Services.Especes;
using AspNetBiodiv.Core.Web.Services.Observations;
using AspNetBiodiv.Core.Web.Services.Statistiques;
using Microsoft.EntityFrameworkCore;

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
            .UseSqlServer(builder.Configuration.GetConnectionString("Especes"));
        //.EnableDetailedErrors();
    });

    builder.Services.AddScoped<IImportService, ImportService>();
    builder.Services.AddScoped<ITaxonomie, DbTaxonomie>();
    builder.Services.AddScoped<IObservations, DbObservations>();
}

builder.Services.AddSingleton<ICommunes, StaticCommunes>();
builder.Services.AddSingleton<IStatisticsService, StatisticsService>();
builder.Services.AddSingleton<AcmeVersionMiddleware>(); 
builder.Services.AddRazorPages();


var app = builder.Build();

app.UseAcmeVersion();

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
app.MapRazorPages();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
