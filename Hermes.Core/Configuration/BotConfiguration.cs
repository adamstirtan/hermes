namespace Hermes.Core.Configuration
{
    public class BotConfiguration : IBotConfiguration
    {
        public BotConfiguration(string fileName)
        { }

        public DiscordCredentials Credentials { get; set; }
        public BotIdentity Identity { get; set; }
    }
}