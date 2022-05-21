using System;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Hermes.Database.Repositories;
using Hermes.Models;

namespace Hermes
{
    public class HermesBot : IBot
    {
        private ServiceProvider _serviceProvider;

        private readonly IConfiguration _configuration;
        private readonly ILogger<HermesBot> _logger;
        private readonly IMessageRepository _messageRepository;
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;

        public HermesBot(
            ILogger<HermesBot> logger,
            IConfiguration configuration,
            IMessageRepository messageRepository)
        {
            _configuration = configuration;
            _logger = logger;
            _messageRepository = messageRepository;

            _client = new DiscordSocketClient();
            _commands = new CommandService();
        }

        public async Task StartAsync(ServiceProvider services)
        {
            _serviceProvider = services;

            _client.MessageReceived += HandleCommandAsync;

            await _commands.AddModulesAsync(Assembly.GetExecutingAssembly(), services);

            await _client.LoginAsync(TokenType.Bot, _configuration["DISCORD_TOKEN"]);
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
            if (arg is not SocketUserMessage message || message.Author.IsBot)
            {
                return;
            }

            _logger.LogInformation(message.ToString());

            int position = 0;

            if (message.HasCharPrefix('!', ref position))
            {
                await _commands.ExecuteAsync(
                    new SocketCommandContext(_client, message),
                    position,
                    _serviceProvider);

                return;
            }

            try
            {
                _messageRepository.Create(new Message
                {
                    Content = message.Content,
                    User = message.Author.Username,
                    Created = DateTime.UtcNow
                });
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}