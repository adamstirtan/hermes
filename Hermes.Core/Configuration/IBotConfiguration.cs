namespace Hermes.Core.Configuration
{
    public interface IBotConfiguration
    {
        DiscordCredentials Credentials { get; set; }

        BotIdentity Identity { get; set; }
    }
}