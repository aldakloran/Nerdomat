using System;
using System.Collections.Generic;
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
using Nerdomat.Modules;
using Microsoft.Extensions.Options;
using Victoria;

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
            await services.GetRequiredService<IMusicService>().InitializeAsync();
            await services.GetRequiredService<CommandHandlingService>().InitializeAsync();
            await services.GetRequiredService<IWatchdogService>().InitializeAsync();

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
                .AddSingleton<ILoggerService, LoggerService>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<HttpClient>()
                // .AddSingleton<LavaNode>()
                // .AddSingleton<LavaConfig>()
                .AddLavaNode(x =>
                {
                    x.SelfDeaf = false;
                })
                .AddSingleton<IMusicService, MusicService>()
                .AddSingleton<IGoogleService, GoogleService>()
                .AddSingleton<IDiscordContextService, DiscordContextService>()
                .AddSingleton<IWarcraftLogsService, WarcraftLogsService>()
                .AddSingleton<IRaiderIoService, RaiderIoService>()
                .AddSingleton<IWatchdogService, WatchdogService>()
                .Configure<Config>(opt =>
                {
                    opt.MyGuildId = 253857543170818049ul;
                    opt.AdminChannelId = 294490276842700811ul;
                    opt.GeneralChannelId = 253857543170818049ul;
                    opt.TestChannelId = 272326889169747973ul;
                    opt.FlaskChannelId = Debugger.IsAttached ? 272326889169747973ul : 566694643211960342ul;    //TestChannelId in debug mode
                    opt.DefaultUserRole = 273155982652211200ul;
                    opt.LogChannelId = 649975460431921188ul;
                    opt.WatchdogMaxFailLimit = 5;
                    opt.GuildData = new GuildData
                    {
                        GuildName = "N E R D",
                        GuildUrl = "https://www.gildia-nerd.pl/",
                        GuildRaidLeadreId = 217034282193911808ul,
                        GuildRaidLeaderName = "Perco"
                    };
                    opt.GoogleSettings = new GoogleSettings
                    {
                        ApplicationName = "Google Sheets API .NET Quickstart",
                        SpreadsheetId = "1eZnKZLNVRmtjkfGkVXwD4xpYA_BYrUyaggcGEPKBR2Y",
                        ServiceAccountEmail = "nerdbot@quickstart-1555516131984.iam.gserviceaccount.com",
                        Jsonfile = "quickstart-1555516131984-9827913e7caa.json",
                        Scopes = new[] { SheetsService.Scope.Spreadsheets },
                        ReportChartUrl = @"https://docs.google.com/spreadsheets/d/e/2PACX-1vTmaZQFUK8-k6n16zVFMbvtznqQbDKJNSO9gQIEHnFaqFG_m_cvf9QtnFTUzXW0Tv4xibp057RkNosV/pubchart?oid=479166207&format=image",
                        FlaskData = new FlaskData
                        {
                            ReportDateFormat = @"MM/dd/yy (hh:mm tt)",
                            ReportDateAddres = "Alchemia!J2",
                            ReportValuesAddres = "Alchemia!E2:G",
                            ReportSubstarctDateAddres = "Alchemia!R2",
                            ReportFlaskCountAddresTemplate = "Alchemia!G2:G{0}"
                        },
                        NerdsData = new NerdsData
                        {
                            ConfigAltRowsAddres = "Alchemia!AE1:AE1",
                            ConfigAltColumnsAddres = "Alchemia!AE2:AE2",
                            MainData = "Alchemia!E2:F",
                            AltsDataOffset = 35,
                            AltsDataTemplate = "Alchemia!AI1:{0}{1}",
                        },
                        NewMembers = new NewMembers
                        {
                            ConfigLastRow = "Alchemia!AE3:AE3",
                            NewMembersAddresTemplate = "Alchemia!L{0}:O{1}"
                        }
                    };
                    opt.WarcraftLogs = new WarcraftLogs
                    {
                        Url = @"https://www.warcraftlogs.com:443/v1/",
                        PrivateKey = @"204d4b00bf29135a94b20fdc9b607555",
                        PublicKey = @"63e64ea36d09230c2be8bb9bb0374ff4",
                        IgnoreTypes = new List<string> { "NPC", "Pet" },
                        FightKeyRegex = @"^.*reports/(.*)/$",
                        PaidEmaoteCode = "\uD83C\uDDF5",    // https://www.fileformat.info/info/unicode/char/1f1f5/index.htm
                        DoneEmoteCode = "\u2611"            // https://www.fileformat.info/info/unicode/char/1f1f5/index.htm
                    };
                    opt.RaidbotsAccountData = new Raidbots
                    {
                        LoginCellAddress = @"Raidbots!B2",
                        PasswordCellAddress = @"Raidbots!B3",
                        AllowerRoleId = 255245178846052363ul
                    };
                })
                .BuildServiceProvider();
        }
    }
}
