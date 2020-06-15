using Aot.Hrms.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;

namespace Aot.Hrms.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .WriteTo.Logger(
                    x => x.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
                        .WriteTo.File($"Logs/Error.log", rollingInterval: RollingInterval.Day)
                )
                .WriteTo.Logger(
                    x => x.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning)
                        .WriteTo.File($"Logs/Warning.log", rollingInterval: RollingInterval.Day)
                )
                .WriteTo.Logger(
                    x => x.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug)
                        .WriteTo.File($"Logs/Debug.log", rollingInterval: RollingInterval.Day)
                )
                .WriteTo.Logger(
                    x => x.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
                        .WriteTo.File($"Logs/Information.log", rollingInterval: RollingInterval.Day)
                )
                .WriteTo.File($"Logs/All.log", rollingInterval: RollingInterval.Day)
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Information("Starting web host");

                var config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", optional: false)
               .Build();

                AotDBContext.Initialize(config["SecurityConfiguraiton:HashKey"]);
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
           
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
