using LoginAndRegister.Data;
using LoginAndRegister.Models;
using LoginAndRegister.Reposaitory;
using LoginAndRegister.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NToastNotify;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

namespace LoginAndRegister
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;
            builder.Services.AddAuthentication(options => {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            }).AddCookie()
            .AddGoogle(GoogleDefaults.AuthenticationScheme, googleOptions =>
            {
                googleOptions.ClientId = configuration["GoogleAuthSettings:Google:ClientId"];
                
                googleOptions.ClientSecret = configuration["GoogleAuthSettings:Google:ClientSecret"];
                googleOptions.CallbackPath = new PathString("/signin-google");
            });

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddIdentity<ApplicationUsers, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();
            builder.Services.AddTransient<IEmailSender, EmailSender>();
            builder.Services.AddTransient<IHomeReposaitory, HomeReposaitory>();
            builder.Services.AddTransient<ICartRepository, CartRepository>();
            builder.Services.AddTransient<IUserOrderRepository, UserOrderRepository>();
            builder.Services.AddMvc().AddNToastNotifyToastr(new NToastNotify.ToastrOptions()
            {
                ProgressBar = true,
                PositionClass = ToastPositions.TopRight,
                PreventDuplicates = true,
                CloseButton = true,


            });
            //builder.Services.AddAuthentication().AddGoogle(options => {
            //    options.ClientId = builder.Configuration.GetSection("GoogleAuthSettings").GetValue<string>("ClientId");
            //    options.ClientSecret = builder.Configuration.GetSection("GoogleAuthSettings").GetValue<string>("ClientSecret");
            //    //options.ClientId = configurationSection["ClientId"];
            //    //options.ClientSecret = configurationSection["ClientSecret"];
            //});

         


            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
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
            app.MapRazorPages();

            app.Run();
        }
    }
}