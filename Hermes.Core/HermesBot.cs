using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Hermes.Core.Commands;
using Hermes.Core.Configuration;

namespace Hermes.Core
{
    public class HermesBot : IBot
    {
        private readonly IBotConfiguration _botConfiguration;
        private readonly DiscordSocketClient _client;

        private CommandHandler _handler;

        public HermesBot(IBotConfiguration botConfiguration)
        {
            _botConfiguration = botConfiguration;

            _client = new DiscordSocketClient();
        }

        public async Task StartAsync()
        {
            var commandService = new CommandService();

            _handler = new CommandHandler(_client, commandService);

            await _handler.InstallCommandsAsync();

            await _client.LoginAsync(TokenType.Bot, _botConfiguration.Credentials.Token);
            await _client.StartAsync();
        }

        public async Task StopAsync()
        {
            if (_client != null)
            {
                await _client.LogoutAsync();
                await _client.StopAsync();
            }
        }
    }
}