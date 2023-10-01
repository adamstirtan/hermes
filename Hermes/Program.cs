using System;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
            var services = CreateServices();

            ApplicationDbContext context = services.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();

            IBot bot = services.GetRequiredService<IBot>();
            await bot.StartAsync(services);

            await Task.Delay(-1);
        }

        private static ServiceProvider CreateServices()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .AddUserSecrets(Assembly.GetExecutingAssembly())
                .Build();

            string connectionString = configuration.GetConnectionString("Default");

            var serviceProvider = new ServiceCollection()
                .AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseSqlite(connectionString);
                })
                .AddLogging(options =>
                {
                    options.ClearProviders();
                    options.AddConsole();
                })
                .AddSingleton<IConfiguration>(configuration)
                .AddScoped<IBot, HermesBot>()
                .AddScoped<IMessageRepository, MessageRepository>()
                .BuildServiceProvider();

            return serviceProvider;
        }
    }
}