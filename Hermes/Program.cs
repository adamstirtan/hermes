using System.Threading.Tasks;

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
            await new HermesBot(
                new BotConfiguration("hermes.config.json"), args).StartAsync();
        }
    }
}