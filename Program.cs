﻿using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Google.Apis.Sheets.v4;
using Nerdomat.Interfaces;
using Nerdomat.Services;
using Nerdomat.Models;
using Nerdomat.Tools;

namespace Nerdomat
{
    internal class Program
    {
        static void Main(string[] args)
                    => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            // You should dispose a service provider created using ASP.NET
            // when you are finished using it, at the end of your app's lifetime.
            // If you use another dependency injection framework, you should inspect
            // its documentation for the best way to do this.
            await using var services = ConfigureServices();
            var client = services.GetRequiredService<DiscordSocketClient>();

            client.Log += LogAsync;
            services.GetRequiredService<CommandService>().Log += LogAsync;

            // Tokens should be considered secret data and never hard-coded.
            // We can read from the environment variable to avoid hardcoding.
            //var o = Environment.GetEnvironmentVariable("DISCORD-TOKEN");
            await client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("DISCORD-TOKEN"));
            await client.StartAsync();

            // Here we initialize the logic required to register our commands.
            await services.GetRequiredService<CommandHandlingService>().InitializeAsync();

            await Task.Delay(Timeout.Infinite);
        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());

            return Task.CompletedTask;
        }

        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<HttpClient>()
                .AddSingleton<IGoogleService, GoogleService>()
                .AddSingleton<ILoggerService, LoggerService>()
                .AddSingleton<IDiscordContextService, DiscordContextService>()
                .Configure<Config>(opt =>
                {
                    opt.MyGuildId = 253857543170818049ul;
                    opt.TestChannelId = 272326889169747973ul;
                    opt.FlaskChannelId = Debugger.IsAttached ? 272326889169747973ul : 566694643211960342ul;    //TestChannelId in debug mode
                    opt.DefaultUserRole = 273155982652211200ul;
                    opt.LogChannelId = 649975460431921188ul;
                    opt.GoogleSettings = new GoogleSettings
                    {
                        ApplicationName = "Google Sheets API .NET Quickstart",
                        SpreadsheetId = "1OBHG73kqjPjchancWMItV6d8tFSFIdLaY4Z7ShPEnWU",
                        ServiceAccountEmail = "nerdbot@quickstart-1555516131984.iam.gserviceaccount.com",
                        Jsonfile = "quickstart-1555516131984-9827913e7caa.json",
                        Scopes = new[] { SheetsService.Scope.Spreadsheets },
                        ReportChartUrl = @"https://docs.google.com/spreadsheets/d/e/2PACX-1vSPNRbKY3mVr-scEmL9APyM4h1UH1moPNZVwKdQgjl3Vj_ZwLo3tu8ilIE8VgAIF6iOJtQemKPRDkbX/pubchart?oid=1599046376&format=image",
                        FlaskData = new FlaskData
                        {
                            ReportDateAddres = "Alchemia!J2",
                            ReportValuesAddres = "Alchemia!E2:G"
                        }
                    };
                })
                .BuildServiceProvider();
        }
    }
}
