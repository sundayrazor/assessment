using Microsoft.AspNetCore.Identity;
using OrderApp.Models;

namespace OrderApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
	        var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddMemoryCache();
            builder.Services.AddMvc();

            //builder.Services.AddSession(options =>
            //{
	           // options.IdleTimeout = TimeSpan.FromSeconds(60);
	           // options.Cookie.HttpOnly = true;
	           // options.Cookie.IsEssential = true;
            //});

            builder.Services.AddSession(options => {
	            options.IdleTimeout = TimeSpan.FromMinutes(30);
            });
            

            var app = builder.Build();
            
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthorization();
            app.UseSession();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}