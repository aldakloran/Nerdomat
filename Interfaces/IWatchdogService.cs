using System.Threading.Tasks;
using System.Timers;

namespace Nerdomat.Interfaces
{
    public interface IWatchdogService
    {
        Task InitializeAsync();
        Task RestartClient();
    }
}