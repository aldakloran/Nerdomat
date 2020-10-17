using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
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
        
        public Lazy<SheetsService> SheetsService { get; private set; }
        
        public GoogleService(IServiceProvider services, IOptionsMonitor<Config> config)
        {
            _services = services;
            _config = config;

            SheetsService = new Lazy<SheetsService>(CreateGoogleSheetsServiceAsync);
        }

        private SheetsService CreateGoogleSheetsServiceAsync()
        {
            ServiceAccountCredential credential;
            using (Stream stream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), _config.CurrentValue.GoogleSettings.Jsonfile), FileMode.Open, FileAccess.Read, FileShare.Read)) {
                credential = (ServiceAccountCredential)
                    GoogleCredential.FromStream(stream).UnderlyingCredential;

                var initializer = new ServiceAccountCredential.Initializer(credential.Id) {
                    User   = _config.CurrentValue.GoogleSettings.ServiceAccountEmail,
                    Key    = credential.Key,
                    Scopes = _config.CurrentValue.GoogleSettings.Scopes
                };
                credential = new ServiceAccountCredential(initializer);
            }

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer {
                HttpClientInitializer = credential,
                ApplicationName       = _config.CurrentValue.GoogleSettings.ApplicationName
            });

            return service;
        }

        public Task<UpdateValuesResponse> WriteDataAsync(string rangeData, ValueRange valueRange) => SheetsService.Value.WriteDataAsync(_config.CurrentValue.GoogleSettings.SpreadsheetId, rangeData, valueRange);
        public Task<List<List<string>>> ReadDataAsync(string rangeData) => SheetsService.Value.ReadDataAsync(_config.CurrentValue.GoogleSettings.SpreadsheetId, rangeData);
        public Task<string> ReadCellAsync(string rangeData) => SheetsService.Value.ReadCellAsync(_config.CurrentValue.GoogleSettings.SpreadsheetId, rangeData);
        public Task<List<T>> ReadDataAsync<T>(string rangeData) where T : class => SheetsService.Value.ReadDataAsync<T>(_config.CurrentValue.GoogleSettings.SpreadsheetId, rangeData);
    }
}