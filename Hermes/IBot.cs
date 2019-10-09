using System.Threading.Tasks;

namespace Hermes
{
    public interface IBot
    {
        Task StartAsync();

        Task StopAsync();
    }
}