using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nerdomat.Interfaces;
using Nerdomat.Models;
using Nerdomat.Tools;

namespace Nerdomat.Services
{
    public class GoogleService : IGoogleService
    {
        private readonly IOptionsMonitor<Config> _config;
        private readonly IServiceProvider _services;
        private readonly ILoggerService _logger;


        public Lazy<SheetsService> SheetsService { get; private set; }

        public GoogleService(IServiceProvider services, IOptionsMonitor<Config> config)
        {
            _logger = services.GetRequiredService<ILoggerService>();
            _services = services;
            _config = config;

            SheetsService = new Lazy<SheetsService>(CreateGoogleSheetsService);

            _logger.WriteLog($"{GetType().Name} initialized");
        }

        private SheetsService CreateGoogleSheetsService()
        {
            ServiceAccountCredential credential;
            using (Stream stream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), _config.CurrentValue.GoogleSettings.Jsonfile), FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                credential = (ServiceAccountCredential)
                    GoogleCredential.FromStream(stream).UnderlyingCredential;

                var initializer = new ServiceAccountCredential.Initializer(credential.Id)
                {
                    User = _config.CurrentValue.GoogleSettings.ServiceAccountEmail,
                    Key = credential.Key,
                    Scopes = _config.CurrentValue.GoogleSettings.Scopes
                };
                credential = new ServiceAccountCredential(initializer);
            }

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = _config.CurrentValue.GoogleSettings.ApplicationName
            });

            return service;
        }

        public Task<UpdateValuesResponse> WriteDataAsync(string rangeData, ValueRange valueRange) => SheetsService.Value.WriteDataAsync(_config.CurrentValue.GoogleSettings.SpreadsheetId, rangeData, valueRange);

        public Task<UpdateValuesResponse> WriteDataAsync(string rangeData, object data)
        {
            var valueRange = new ValueRange();
            valueRange.Values = new List<IList<object>> { new List<object> { data } };

            return WriteDataAsync(rangeData, valueRange);
        }

        public Task<UpdateValuesResponse> WriteDataAsync(string rangeData, IEnumerable<object> data)
        {
            var valueRange = new ValueRange();
            valueRange.Values = data.Select(item => new List<object>() { item }).Cast<IList<object>>().ToList();

            return WriteDataAsync(rangeData, valueRange);
        }
        public Task<List<List<string>>> ReadDataAsync(string rangeData) => SheetsService.Value.ReadDataAsync(_config.CurrentValue.GoogleSettings.SpreadsheetId, rangeData);

        public Task<string> ReadCellAsync(string rangeData) => SheetsService.Value.ReadCellAsync(_config.CurrentValue.GoogleSettings.SpreadsheetId, rangeData);

        public Task<List<T>> ReadDataAsync<T>(string rangeData) where T : class => SheetsService.Value.ReadDataAsync<T>(_config.CurrentValue.GoogleSettings.SpreadsheetId, rangeData);

        public async Task<List<NerdModel>> GetNerdsAsync()
        {
            var nerdConfig = _config.CurrentValue.GoogleSettings.NerdsData;
            var rows = (await ReadCellAsync(nerdConfig.ConfigAltRowsAddres)).GetInt();
            var columns = (await ReadCellAsync(nerdConfig.ConfigAltColumnsAddres)).GetInt();

            var firstColumn = (nerdConfig.AltsDataOffset + columns).GetExcelColumnName();
            var altsAddres = string.Format(nerdConfig.AltsDataTemplate, firstColumn, rows);

            var mainData = await ReadDataAsync(nerdConfig.MainData);
            var altData = await ReadDataAsync(altsAddres);

            var data = mainData.Select(x => new NerdModel { DiscordTag = x.FirstOrDefault(), MainNick = x.LastOrDefault(), Handled = false }).ToList();
            Parallel.ForEach(data, item =>
            {
                foreach (var alts in altData)
                {
                    if (alts.Where(x => !string.IsNullOrEmpty(x)).Any(x => string.Equals(x, item.MainNick, StringComparison.OrdinalIgnoreCase)))
                    {
                        item.AllNicks = alts.Where(x => !string.IsNullOrEmpty(x)).ToList();
                        break;
                    }
                }

                item.AllNicks ??= new List<string> { item.MainNick };
            });

            return data;
        }
    }
}