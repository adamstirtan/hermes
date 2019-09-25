using System;
using System.Threading.Tasks;

using Hermes.Core.Configuration;

namespace Hermes.Core
{
    public class HermesBot : IBot
    {
        private readonly IBotConfiguration _botConfiguration;

        public HermesBot(IBotConfiguration botConfiguration, string[] args)
        {
            _botConfiguration = botConfiguration;
        }

        public Task StartAsync()
        {
            throw new NotImplementedException();
        }

        public Task StopAsync()
        {
            throw new NotImplementedException();
        }
    }
}