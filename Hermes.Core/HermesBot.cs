using System.Reflection;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Hermes.Core.Configuration;

namespace Hermes.Core
{
    public class HermesBot : IBot
    {
        private readonly IBotConfiguration _botConfiguration;
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;

        public HermesBot(IBotConfiguration botConfiguration)
        {
            _botConfiguration = botConfiguration;

            _client = new DiscordSocketClient();
            _commands = new CommandService();
        }

        public async Task StartAsync()
        {
            _client.MessageReceived += HandleCommandAsync;

            await _commands.AddModulesAsync(assembly: Assembly.GetExecutingAssembly(), services: null);

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

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            if (!(arg is SocketUserMessage message))
            {
                return;
            }

            int argPos = 0;

            if (!(message.HasCharPrefix('!', ref argPos) ||
                message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
            {
                return;
            }

            var context = new SocketCommandContext(_client, message);

            await _commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: null);
        }
    }
}