using System;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

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

            using (var db = _factory.CreateDbContext())
            {
                db.Database.Migrate();
            }
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

            if (message.Author.IsBot)
            {
                return;
            }

            int position = 0;

            if (message.HasCharPrefix('!', ref position))
            {
                var context = new SocketCommandContext(_client, message);

                await _commands.ExecuteAsync(context, position, null);
            }
            else
            {
                using (var db = _factory.CreateDbContext())
                {
                    try
                    {
                        await db.Messages.AddAsync(new Message
                        {
                            Content = message.Content,
                            User = message.Author.Username,
                            Created = DateTime.UtcNow
                        });

                        db.SaveChanges();
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                    }
                }
            }
        }
    }
}