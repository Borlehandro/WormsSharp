using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WormsApp.domain.services;
using WormsApp.Domain.Services;
using WormsApp.domain.strategy;

namespace WormsApp
{
    class Launcher
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
                    services.AddSingleton<INamesGenerator, SequenceNamesGenerator>(_ => new SequenceNamesGenerator());
                    services.AddScoped<IStrategy, NearestFoodStrategy>(_ => new NearestFoodStrategy());
                })
                .UseConsoleLifetime();
        }
    }
}