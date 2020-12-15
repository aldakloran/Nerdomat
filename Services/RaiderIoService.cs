using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nerdomat.Interfaces;
using Nerdomat.Models;
using Nerdomat.Tools;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Nerdomat.Services
{
    public class RaiderIoService : IRaiderIoService
    {
        private readonly IOptionsMonitor<Config> _config;
        private readonly HttpClient _httpClient;
        private readonly IServiceProvider _services;
        private readonly ILoggerService _logger;

        private const string RaiderIoApi = @"https://raider.io/api/v1/";

        public RaiderIoService(IServiceProvider services, IOptionsMonitor<Config> config)
        {
            _httpClient = services.GetRequiredService<HttpClient>();
            _logger = services.GetRequiredService<ILoggerService>();
            _services = services;
            _config = config;

            _logger.WriteLog($"{GetType().Name} initialized");
        }

        public async Task<RioCharacterProfileModel> ProfileGet(string characterName)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "region", "eu" },
                { "realm", "burning-legion" },
                { "name", characterName },
                { "fields", "gear,covenant,raid_progression,mythic_plus_ranks,mythic_plus_scores_by_season:current" }
            };

            var query = QueryHelpers.AddQueryString(string.Concat(RaiderIoApi, @"characters/profile/"), queryParams);
            using (var request = new HttpRequestMessage(HttpMethod.Get, query))
            {
                var rioData = await _httpClient.SendAsync(request);
                if (rioData.IsSuccessStatusCode)
                {
                    var jsonResolver = new PropertyRenameAndIgnoreSerializerContractResolver();
                    jsonResolver.NamingStrategy = new SnakeCaseNamingStrategy();

                    var jss = new JsonSerializerSettings
                    {
                        ContractResolver = jsonResolver,
                        Formatting = Formatting.None
                    };

                    var jsonData = await rioData.Content.ReadAsStringAsync();
                    var validJsonData = jsonData.Replace("\"castle-nathria\"", "\"castle_nathria\"");
                    var fights = JsonConvert.DeserializeObject<RioCharacterProfileModel>(validJsonData, jss);

                    return fights;
                }
                else
                {
                    await _logger.WriteLog($"Fail in Rio request {rioData.ReasonPhrase} code {rioData.StatusCode}");
                    return null;
                }
            }
        }
    }
}