using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WormsApp.Domain.Services;

namespace WormsApp
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Start();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args).ConfigureServices((_, services) =>
                {
                    services.AddHostedService<GameService>();
                    services.AddSingleton<ILogger, SimpleLogger>(_ => new SimpleLogger(new StreamWriter(args[0])));
                    services.AddScoped<FoodGenerator>();
                    services.AddScoped<NamesGenerator>();
                })
                .UseConsoleLifetime();
        }
    }
}