using System;
using System.IO;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Hermes.Core;
using Hermes.Core.Configuration;

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
                DisplayUsage();
                return;
            }

            var botConfiguration = JsonConvert.DeserializeObject<BotConfiguration>(
                File.ReadAllText(Path.Combine(GetConfigurationDirectory(), "config.json")));

            await new HermesBot(botConfiguration).StartAsync();
        }

        private static void DisplayUsage()
        {
            Console.WriteLine("\nHermes is a Discord bot for doing fun things with friends.");
            Console.WriteLine("  Usage: hermes [JSON config file]\n");
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