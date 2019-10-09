using System;
using System.Reflection;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Hermes.Configuration;
using Hermes.Database;
using Hermes.Models;

namespace Hermes
{
    public class HermesBot : IBot
    {
        private readonly IBotConfiguration _configuration;
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly DbContextFactory _factory;

        public HermesBot(IBotConfiguration configuration)
        {
            _configuration = configuration;

            _client = new DiscordSocketClient();
            _commands = new CommandService();
            _factory = new DbContextFactory();
        }

        public async Task StartAsync()
        {
            _client.MessageReceived += HandleCommandAsync;

            await _commands.AddModulesAsync(Assembly.GetExecutingAssembly(), null);

            await _client.LoginAsync(TokenType.Bot, _configuration.Credentials.Token);
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
                using (var db = _factory.CreateDbContext(null))
                {
                    await db.Messages.AddAsync(new Message
                    {
                        Content = message.Content,
                        User = message.Author.Username,
                        Created = DateTime.UtcNow
                    });

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }

                return;
            }

            var context = new SocketCommandContext(_client, message);

            await _commands.ExecuteAsync(context, argPos, null);
        }
    }
}