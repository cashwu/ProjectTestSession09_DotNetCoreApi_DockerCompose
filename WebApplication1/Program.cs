using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)                   
                   .ConfigureAppConfiguration
                   (
                       (builderContext, config) =>
                       {
                           IHostingEnvironment env = builderContext.HostingEnvironment;

                           config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                           config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                           config.AddEnvironmentVariables();
                       }
                   )
                   .UseStartup<Startup>()
                   .Build();
    }
}