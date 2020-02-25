using System;
using System.IO;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Hermes.Configuration;

namespace Hermes
{
    internal class Program
    {
        private static void Main(string[] args) =>
            MainAsync(args).GetAwaiter().GetResult();

        private static async Task MainAsync(string[] args)
        {
            var botConfiguration = JsonConvert.DeserializeObject<BotConfiguration>(
                File.ReadAllText(Path.Combine(GetConfigurationDirectory(), "config.json")));

            var bot = new HermesBot(botConfiguration);

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

        private static string GetConfigurationDirectory()
        {
            var baseDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

            if (baseDirectory.Parent?.Parent?.Parent != null)
            {
                return baseDirectory.Parent.Parent.Parent.FullName;
            }

            throw new DirectoryNotFoundException();
        }
    }
}