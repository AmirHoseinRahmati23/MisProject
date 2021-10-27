global using DbLayer;
global using DbLayer.Contexts;
global using DbLayer.Entities;
global using DbLayer.Entities.Users;
global using DbLayer.Entities.Permissions;

global using Core;
global using Core.Security;
global using Core.Services;
global using Core.Services.Interfaces;

global using ClientLibraries;
global using ClientLibraries.DTOs;
global using ClientLibraries.Enums;
global using ClientLibraries.Converters;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Mis API",
        Description = "Mis API working for blazor application",
        TermsOfService = new Uri("https://github.com/drit-group/misproject"),
        Contact = new OpenApiContact
        {
            Name = "Taha Ghadirian",
            Email = "Taha.Ghadirian@outook.com",
            Url = new Uri("https://t.me/tahat4tt"),
        },
        License = new OpenApiLicense
        {
            Name = "Use under GPL-3",
            Url = new Uri("https://github.com/drit-group/misproject/LICENSE"),
        },
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddDbContext<MisDbContext>(p =>
    p.UseSqlServer(builder.Configuration.GetConnectionString("MisDbContext")));
builder.Services.AddTransient<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI(p =>
{
    p.SwaggerEndpoint("/swagger/v1/swagger.json", "MIS API V1");
});

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
