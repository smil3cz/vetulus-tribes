using GreenFoxAcademy.SpaceSettlers.Database;
using GreenFoxAcademy.SpaceSettlers.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace GreenFoxAcademy.SpaceSettlers
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            //EnsureCreated(host);

            host.Run();
        }
        private static void EnsureCreated(IHost host)
        {
            var scope = host.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
            dbContext.Database.EnsureCreated();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static void CreateLogFile()
        {
            var logFilePath = AppSettings.LogFilePath;
            if (!Directory.Exists(Path.GetDirectoryName(logFilePath)) || !File.Exists(logFilePath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));
                File.Create(logFilePath).Close();
            }
        }
    }
}
