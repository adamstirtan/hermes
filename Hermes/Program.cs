using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Hermes.Database;

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

            Assembly assembly = Assembly.GetExecutingAssembly();
            string basePath = Path.GetDirectoryName(assembly.Location);

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
                .BuildServiceProvider();

            HermesBot bot = new(configuration);

            try
            {
                await bot.StartAsync();

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