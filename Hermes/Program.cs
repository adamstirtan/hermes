using System;
using System.IO;
using System.Reflection;
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
            if (args.Length != 1)
            {
                Console.WriteLine("Expected single argument of path to config file");
                Environment.Exit(-1);
            }

            var assembly = Assembly.GetExecutingAssembly();
            var basePath = Path.GetDirectoryName(assembly.Location);

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddUserSecrets(assembly)
                .Build();

            string connectionString = configuration["ConnectionStrings:DefaultConnection"];

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
                    int key = Console.Read();

                    if (key == (int)ConsoleKey.Escape)
                    {
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