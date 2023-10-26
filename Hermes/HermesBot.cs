using System;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using FluentScheduler;

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
        private readonly ulong _announcementChannelId;

        public HermesBot(
            ILogger<HermesBot> logger,
            IConfiguration configuration,
            IMessageRepository messageRepository)
        {
            _configuration = configuration;
            _logger = logger;
            _messageRepository = messageRepository;

            DiscordSocketConfig config = new()
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
            };

            _client = new DiscordSocketClient(config);
            _commands = new CommandService();

            _announcementChannelId = ulong.Parse(_configuration["AnnouncementChannelID"]);
        }

        public async Task StartAsync(ServiceProvider services)
        {
            string discordToken = _configuration["DiscordToken"] ?? throw new Exception("Missing Discord Token");

            _logger.LogInformation($"Discord Token: {discordToken}");

            _serviceProvider = services;

            _client.MessageReceived += HandleCommandAsync;

            await _commands.AddModulesAsync(Assembly.GetExecutingAssembly(), services);

            await _client.LoginAsync(TokenType.Bot, discordToken);
            await _client.StartAsync();

            _logger.LogInformation("Connected to Discord successfully");

            _client.Ready += OnClientReady;
        }

        private Task OnClientReady()
        {
            JobManager.Initialize();

            JobManager.AddJob(async () =>
            {
                EmbedBuilder builder = new();

                builder.AddField("Game Night!", "1 hour until we start. Charge your vape, controllers and headphones and install game updates!");
                builder.WithThumbnailUrl("https://media1.giphy.com/media/MdeSslU80bVsTtWxEh/giphy.gif?cid=ecf05e47rm6ietidzhpi881lkimj701xaslz6kxr8kasqis9&ep=v1_gifs_search&rid=giphy.gif&ct=g");
                builder.WithColor(Color.Red);

                var channel = _client.GetChannel(_announcementChannelId) as IMessageChannel;
                await channel.SendMessageAsync(embed: builder.Build());

            }, schedule => schedule.ToRunEvery(0).Weeks().On(DayOfWeek.Wednesday).At(20, 0));

            JobManager.AddJob(async () =>
            {
                EmbedBuilder builder = new();

                builder.AddField("Game Night!", "This is your 15 minute warning. Find your AirPods, grab a Zevia and voice up.");
                builder.WithThumbnailUrl("https://media4.giphy.com/media/3orifckBVb4KdqpUqs/giphy.gif?cid=ecf05e47wr8llc242jtpsj5zuz3q12vem5jsez3947p0o7ef&ep=v1_gifs_search&rid=giphy.gif&ct=g");
                builder.WithColor(Color.Red);

                var channel = _client.GetChannel(_announcementChannelId) as IMessageChannel;
                await channel.SendMessageAsync(embed: builder.Build());

            }, schedule => schedule.ToRunEvery(0).Weeks().On(DayOfWeek.Wednesday).At(20, 45));

            return Task.CompletedTask;
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
            bool messageIsCommand = message.HasCharPrefix('!', ref position);

            if (messageIsCommand)
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
                _logger.LogError(exception.Message);
            }
        }
    }
}