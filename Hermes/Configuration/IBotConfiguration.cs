namespace Hermes.Configuration
{
    public interface IBotConfiguration
    {
        DiscordCredentials Credentials { get; set; }

        BotIdentity Identity { get; set; }
    }
}