using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nerdomat.Interfaces;
using Nerdomat.Models;
using Newtonsoft.Json;

namespace Nerdomat.Services
{
    public class WarcraftLogsService : IWarcraftLogsService
    {
        private readonly IOptionsMonitor<Config> _config;
        private readonly HttpClient _httpClient;
        private readonly IServiceProvider _services;
        private readonly ILoggerService _logger;

        private const string WarcraftLogsApi = @"https://www.warcraftlogs.com/v1/";

        public WarcraftLogsService(IServiceProvider services, IOptionsMonitor<Config> config)
        {
            _httpClient = services.GetRequiredService<HttpClient>();
            _logger = services.GetRequiredService<ILoggerService>();
            _services = services;
            _config = config;

            Console.WriteLine($"{GetType().Name} initialized");
        }

        public async Task<WarcraftLogsFightsModel> GetFullFight(string fightId)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "translate", "true" },
                { "api_key", _config.CurrentValue.WarcraftLogs.PublicKey }
            };
            var query = QueryHelpers.AddQueryString(string.Concat(WarcraftLogsApi, @"report/fights/", fightId), queryParams);

            using (var request = new HttpRequestMessage(HttpMethod.Get, query))
            {
                var raidData = await _httpClient.SendAsync(request);
                if (raidData.IsSuccessStatusCode)
                {
                    var jsonData = await raidData.Content.ReadAsStringAsync();
                    var fights = JsonConvert.DeserializeObject<WarcraftLogsFightsModel>(jsonData);

                    return fights;
                }
                else
                {
                    await _logger.WriteLog($"Fail in WL request {raidData.ReasonPhrase} code {raidData.StatusCode}");
                    return null;
                }
            }
        }

        public async Task<List<WarcraftLogsFightsModel>> GetFullFight(IEnumerable<string> fightId)
        {
            var data = new List<WarcraftLogsFightsModel>();
            foreach (var id in fightId)
                data.Add(await GetFullFight(id));

            return data;
        }

        public async Task<List<Friendly>> GetDistinctFriendly(string fightId)
        {
            var data = await GetFullFight(fightId);
            var distinctCharacters = data.Friendlies.Where(x => !_config.CurrentValue.WarcraftLogs.IgnoreTypes.Contains(x.Type))
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Type)
                .Distinct()
                .ToList();

            return distinctCharacters;
        }

        public async Task<List<Friendly>> GetDistinctFriendly(IEnumerable<string> fightId)
        {
            var data = new List<Friendly>();
            foreach (var id in fightId)
                data.AddRange(await GetDistinctFriendly(id));

            var distinctCharacters = data.Where(x => !_config.CurrentValue.WarcraftLogs.IgnoreTypes.Contains(x.Type))
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Type)
                .Distinct()
                .ToList();

            return distinctCharacters;
        }
    }
}