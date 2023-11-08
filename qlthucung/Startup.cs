using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using qlthucung.Models;
using qlthucung.Security;
using System;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("AppDb")));

        services.AddDbContext<AppIdentityDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("AppDb")));


        services.AddIdentity<AppIdentityUser, AppIdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
        })
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddDefaultTokenProviders();

        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Security/SignIn";
            options.AccessDeniedPath = "/Security/AccessDenied";
        });

        services.AddSession();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseSession();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "customerArea",
                pattern: "{area:exists=Customer}/{controller=SanPham}/{action=Index}/{id?}"
            );
            endpoints.MapAreaControllerRoute(
                name: "adminArea",
                areaName: "Admin",
                pattern: "Admin/{controller=Dashboard}/{action=Index}/{id?}"
            );
            endpoints.MapControllerRoute(
                name: "adminCategoryXoaConfirmed",
                pattern: "Admin/Category/XoaConfirmed/{id?}",
                defaults: new { controller = "Category", action = "XoaConfirmed" }
           );

        });

    }
}
