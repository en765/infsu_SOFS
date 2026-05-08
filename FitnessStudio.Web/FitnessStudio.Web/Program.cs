using FitnessStudio.Data.Database;
using FitnessStudio.Data.Interfaces;
using FitnessStudio.Data.Repositories;
using FitnessStudio.Business.Interfaces;
using FitnessStudio.Business.Services;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<DatabaseConnectionFactory>();
builder.Services.AddScoped<IPaketRepository, PaketRepository>();
builder.Services.AddScoped<IPaketService, PaketService>();

builder.Services.AddScoped<IClanRepository, ClanRepository>();
builder.Services.AddScoped<IClanarinaRepository, ClanarinaRepository>();
builder.Services.AddScoped<IUplataRepository, UplataRepository>();

builder.Services.AddScoped<IClanService, ClanService>();
builder.Services.AddScoped<IClanarinaService, ClanarinaService>();
builder.Services.AddScoped<IUplataService, UplataService>();

var app = builder.Build();

var supportedCultures = new[] { new CultureInfo("hr-HR") };

var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("hr-HR"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};

app.UseRequestLocalization(localizationOptions);

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
