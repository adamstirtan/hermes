using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

namespace Hermes
{
    public interface IBot
    {
        Task StartAsync(ServiceProvider services);

        Task StopAsync();
    }
}