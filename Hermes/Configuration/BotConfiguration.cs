namespace Hermes.Configuration
{
    public class BotConfiguration : IBotConfiguration
    {
        public DiscordCredentials Credentials { get; set; }
        public BotIdentity Identity { get; set; }
    }
}