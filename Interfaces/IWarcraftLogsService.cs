using System.Collections.Generic;
using System.Threading.Tasks;
using Nerdomat.Models;

namespace Nerdomat.Interfaces
{
    public interface IWarcraftLogsService
    {
        Task<WarcraftLogsFightsModel> GetFullFight(string fightId);
        Task<List<WarcraftLogsFightsModel>> GetFullFight(IEnumerable<string> fightId);
        Task<List<Friendly>> GetDistinctFriendly(string fightId);
        Task<List<Friendly>> GetDistinctFriendly(IEnumerable<string> fightId);
    }
}