using System.Threading.Tasks;

namespace Hermes.Core
{
    public interface IBot
    {
        Task StartAsync();

        Task StopAsync();
    }
}