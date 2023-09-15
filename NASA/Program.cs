using NASA.Helper;
using NASA.Helper.Contracts;
using NASA.Models.Config;
using NASA.Services;
using NASA.Services.Contracts;
using OfficeOpenXml;
using Serilog;

namespace NASA
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            builder.Services.Configure<ApiKeys>(builder.Configuration.GetSection("ApiKeys"));

            builder.Host.UseSerilog((ctx, lc) => lc
                     .WriteTo.Console());

            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<INasaApis, NasaApis>();
            builder.Services.AddScoped<IAsteroidsOrganizer, AsteroidsOrganizer>();


            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}