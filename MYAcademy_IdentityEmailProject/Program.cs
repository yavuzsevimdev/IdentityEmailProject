
using Microsoft.AspNetCore.Identity;
using MYAcademy_IdentityEmailProject.Context;
using MYAcademy_IdentityEmailProject.Entities;
using MYAcademy_IdentityEmailProject.Models;
using MYAcademy_IdentityEmailProject.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EmailContext>();

builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<EmailContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login/SignIn";
    options.AccessDeniedPath = "/ErrorPages/Page403";
    options.ExpireTimeSpan = TimeSpan.FromDays(7);

    // Giriş yapmamış kullanıcı da 403 sayfasını görsün.
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.Redirect("/ErrorPages/Page403");
        return Task.CompletedTask;
    };

    // Giriş yapmış ama yetkisi olmayan kullanıcı da 403 görsün.
    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.Redirect("/ErrorPages/Page403");
        return Task.CompletedTask;
    };
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/ErrorPages/Page500");
    app.UseHsts();
}



app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

// HTTP durum kodlarını yakala
app.UseStatusCodePagesWithReExecute("/ErrorPages/Page{0}");

app.UseAuthentication();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Message}/{action=Inbox}/{id?}")
    .WithStaticAssets();

app.Run();
