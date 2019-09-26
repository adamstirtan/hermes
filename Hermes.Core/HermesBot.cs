using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;

using Hermes.Core.Configuration;

namespace Hermes.Core
{
    public class HermesBot : IBot
    {
        private readonly IBotConfiguration _botConfiguration;
        private readonly DiscordSocketClient _client;

        public HermesBot(IBotConfiguration botConfiguration)
        {
            _botConfiguration = botConfiguration;

            _client = new DiscordSocketClient();
        }

        public async Task StartAsync()
        {
            await _client.LoginAsync(TokenType.Bot, _botConfiguration.Credentials.Token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        public async Task StopAsync()
        {
            if (_client != null)
            {
                await _client.StopAsync();
            }
        }
    }
}