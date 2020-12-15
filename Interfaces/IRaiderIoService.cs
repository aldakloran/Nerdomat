using System.Threading.Tasks;
using Nerdomat.Models;

namespace Nerdomat.Interfaces
{
    public interface IRaiderIoService
    {
        Task<RioCharacterProfileModel> ProfileGet(string characterName);
    }
}