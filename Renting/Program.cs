using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Renting.Data;
using Renting.Models;
using Renting.Services;

namespace Renting
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<RentingContext>(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
                options.UseSqlServer(connectionString);
            }
            );

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
                options.UseSqlServer(connectionString);
            }
            );

            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 6;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            })

                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddHostedService<RentalCleanupHostedService>();

            var app = builder.Build();

            await SeedService.SeedDataBase(app.Services);

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

            app.Use(async (context, next) =>
            {
                if (context.User.Identity?.IsAuthenticated == true)
                {
                    var db = context.RequestServices.GetRequiredService<RentingContext>();
                    var userId = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                    if (!string.IsNullOrEmpty(userId))
                    {
                        var approvedRental = await db.Rentals
                            .Where(r => r.UserId == userId && r.Status == Statusenum.Approved)
                            .OrderByDescending(r => r.Id)
                            .FirstOrDefaultAsync();

                        if (approvedRental != null)
                        {
                            context.Items["ApprovedRentalMessage"] = "Masz zatwierdzone wypo¿yczenie!";
                        }
                    }
                }
                await next();
            });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
