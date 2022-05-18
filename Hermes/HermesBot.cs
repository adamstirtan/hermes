using System;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Hermes.Database;
using Hermes.Models;

namespace Hermes
{
    public class HermesBot : IBot
    {
        private readonly IConfiguration _configuration;
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly DbContextFactory _factory;

        public HermesBot(IConfiguration configuration)
        {
            _configuration = configuration;

            _client = new DiscordSocketClient();
            _commands = new CommandService();
            _factory = new DbContextFactory();

            using var context = _factory.CreateDbContext();

            context.Database.Migrate();
        }

        public async Task StartAsync()
        {
            _client.MessageReceived += HandleCommandAsync;

            await _commands.AddModulesAsync(Assembly.GetExecutingAssembly(), null);

            await _client.LoginAsync(TokenType.Bot, _configuration["Discord:Token"]);
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
            if (arg is not SocketUserMessage message)
            {
                return;
            }

            if (message.Author.IsBot)
            {
                return;
            }

            int position = 0;

            if (message.HasCharPrefix('!', ref position))
            {
                await _commands.ExecuteAsync(
                    new SocketCommandContext(_client, message),
                    position,
                    null);

                return;
            }

            using var context = _factory.CreateDbContext();

            try
            {
                await context.Messages.AddAsync(new Message
                {
                    Content = message.Content,
                    User = message.Author.Username,
                    Created = DateTime.UtcNow
                });

                context.SaveChanges();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}