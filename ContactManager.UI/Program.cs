using ContactManager.CORE;
using ContactManager.CutomMiddlewares;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repository;
using RepositoryContract;
using Serilog;
using SerivicesContracts;
using Services;
namespace ContactManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) => {

                loggerConfiguration
                .ReadFrom.Configuration(context.Configuration) //read configuration settings from built-in IConfiguration
                .ReadFrom.Services(services); //read out current app's services and make them available to serilog
            });
            builder.Services.serviceDescriptors(builder.Configuration); 
            var app = builder.Build();
            app.UseHsts();
            app.UseHttpsRedirection();
            if (builder.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseCustomErrorHandlingMiddleware();
            }
          
            app.UseSerilogRequestLogging();
            Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "rotativa");
            app.Logger.LogInformation("Logging is working");
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

           

            app.Run();
        }
    }
}
