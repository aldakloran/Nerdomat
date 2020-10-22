using System.Collections.Generic;
using System.Threading.Tasks;
using Nerdomat.Models;

namespace Nerdomat.Interfaces
{
    public interface IWarcraftLogsService
    {
        Task<List<Friendly>> GetCharacters(string fightId);
    }
}