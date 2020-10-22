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

        private const string _warcraftLogsApi = @"https://www.warcraftlogs.com/v1/";

        public WarcraftLogsService(IServiceProvider services, IOptionsMonitor<Config> config)
        {
            _httpClient = services.GetRequiredService<HttpClient>();
            _logger = services.GetRequiredService<ILoggerService>();
            _services = services;
            _config = config;
        }

        public async Task<List<Friendly>> GetCharacters(string fightId)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "translate", "true" },
                { "api_key", _config.CurrentValue.WarcraftLogs.PublicKey }
            };
            var query = QueryHelpers.AddQueryString(string.Concat(_warcraftLogsApi, @"report/fights/", fightId), queryParams);
            
            using (var request = new HttpRequestMessage(HttpMethod.Get, query))
            {
                var raidData = await _httpClient.SendAsync(request);
                if (raidData.IsSuccessStatusCode)
                {
                    var jsonData = await raidData.Content.ReadAsStringAsync();
                    var fights = JsonConvert.DeserializeObject<WarcraftLogsFightsModel>(jsonData);
                    
                    return fights.Friendlies;
                }
                else
                {
                    await _logger.WriteLog($"Fail in WL request {raidData.ReasonPhrase} code {raidData.StatusCode}");
                    return null;
                }
            }
        }
    }
}