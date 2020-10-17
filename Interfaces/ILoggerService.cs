using System.Threading.Tasks;

namespace Nerdomat.Interfaces
{
    public interface ILoggerService
    {
        Task WriteLog(string message);
    }
}