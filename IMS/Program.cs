using DotNetEnv;
using IMS_DomainLayer.Data;
using IMS_DomainLayer.Models;
using IMS_RepositoryLayer.IRepository;
using IMS_RepositoryLayer.Repository;
using IMS_ServiceLayer.IService;
using IMS_ServiceLayer.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;
using System;

namespace IMS
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Env.Load();

            var builder = WebApplication.CreateBuilder(args);
            QuestPDF.Settings.License = LicenseType.Community;

            #region Environment Variables
            builder.Configuration["Supabase:Url"] = Environment.GetEnvironmentVariable("URL");
            builder.Configuration["Supabase:AnonKey"] = Environment.GetEnvironmentVariable("ANON_KEY");
            builder.Configuration["ConnectionStrings:DefaultConnection"] = Environment.GetEnvironmentVariable("CONNECTION_STRING");
            #endregion

            #region Add services to the container
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(builder.Configuration["ConnectionStrings:DefaultConnection"]));

            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            builder.Services.AddScoped<ICustomService<Client>, ClientService>();
            builder.Services.AddScoped<ICustomService<Invoice>, InvoiceService>();
            builder.Services.AddScoped<IInvoicePdfService, InvoicePdfService>();

            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
            }).AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/SignIn";
            });
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            #region Initial DB Setup
            try
            {
                using IServiceScope scope = app.Services.CreateScope();

                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
                var roles = new string[2] { "Business", "Admin" };
                var businessRoleDesc = "Business role has limited permissions to manage their own invoices, clients and payments.";
                var adminRoleDesc = "Admin role has all the permissions to manage the system.";

                foreach (string role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new ApplicationRole
                        {
                            Name = role,
                            IsActive = true,
                            Description = role == "Admin" ? adminRoleDesc : businessRoleDesc,
                        });
                    }
                }
            } 
            catch (Exception e)
            {
                throw new Exception($"An error occurred: {e.Message}");
            }

            try
            {

                using (var scope = app.Services.CreateScope())
                {
                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                    string email = "admin@invoicehub.com";

                    var user = await userManager.FindByEmailAsync(email);

                    if (user == null)
                    {
                        user = new ApplicationUser
                        {
                            FullName = "System Administrator",
                            AddressLine = "32 Invoice Hub, OakLand Street",

                            UserName = email,
                            Email = email,
                            EmailConfirmed = true
                        };

                        await userManager.CreateAsync(user, "AdminPassword#1");
                        await userManager.AddToRoleAsync(user, "Admin");
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception($"An error occurred: {e.Message}");
            }
            #endregion

            await app.RunAsync();
        } // end method
    } // end class
} // end namespace
