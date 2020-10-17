using System;
using System.IO;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Microsoft.Extensions.Options;
using Nerdomat.Models;

namespace Nerdomat.Services
{
    public class GoogleService
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
    }
}