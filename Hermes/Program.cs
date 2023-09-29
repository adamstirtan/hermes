using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Hermes.Database;
using Hermes.Database.Repositories;

namespace Hermes
{
    internal class Program
    {
        private static void Main(string[] args) =>
            MainAsync(args).GetAwaiter().GetResult();

        private static async Task MainAsync(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables("HERMES_")
                .Build();

            string connectionString = configuration["CONNECTION_STRING"];

            var serviceProvider = new ServiceCollection()
                .AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseSqlite(connectionString);
                })
                .AddLogging()
                .AddSingleton<IConfiguration>(configuration)
                .AddScoped<IBot, HermesBot>()
                .AddScoped<IMessageRepository, MessageRepository>()
                .BuildServiceProvider();

            try
            {
                serviceProvider
                    .GetRequiredService<ApplicationDbContext>()
                    .Database.Migrate();

                IBot bot = serviceProvider.GetRequiredService<IBot>();

                await bot.StartAsync(serviceProvider);

                do
                {
                    var keyInfo = Console.ReadKey();

                    if (keyInfo.Key == ConsoleKey.Q)
                    {
                        Console.WriteLine("\nShutting down!");

                        await bot.StopAsync();

                        return;
                    }
                } while (true);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                Environment.Exit(-1);
            }
        }
    }
}