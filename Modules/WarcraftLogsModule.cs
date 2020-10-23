using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nerdomat.Interfaces;
using Nerdomat.Models;
using Nerdomat.Tools;

namespace Nerdomat.Modules
{
    /*
     *   This module is a bit different so let's clarify
     *
     *   The module is mainly used for communication with the WarcraftLogs service, 
     *   but based on the data from this website, we count the flasks, 
     *   so this module is related to both WarcraftLogs and GoogleService and the flask module itself
     *
     *   Example:
     *  
     *       WarcraftLogs related methods:
     *           * GetRaiders
     *           * AddReaction
     *
     *       GoogleService related methods:
     *          * SubstractFlasks
     *       
     *       Hybrid (WarcraftLogs + GoogleService) methods:
     *           * CountFlasks
     */
 
    [ModuleActive(true)]
    [ModuleName("Warcraft Logs")]
    public class WarcraftLogsModule : ModuleBase<SocketCommandContext>
    {
        private readonly IOptionsMonitor<Config> _config;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;
        private readonly IWarcraftLogsService _warcraftLogsService;
        private readonly ILoggerService _logger;
        private readonly IDiscordContextService _discordContext;
        private readonly IGoogleService _googleService;

        public WarcraftLogsModule(IServiceProvider services, IOptionsMonitor<Config> config)
        {
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _warcraftLogsService = services.GetRequiredService<IWarcraftLogsService>();
            _logger = services.GetRequiredService<ILoggerService>();
            _discordContext = services.GetRequiredService<IDiscordContextService>();
            _googleService = services.GetRequiredService<IGoogleService>();
            _services = services;
            _config = config;
        }

        [Command("wszyscy")]
        [Summary("Wyliczenie wszystkich postaci na raidach")]
        public async Task GetRaiders(params string[] ids)
        {
            var charanters = await _warcraftLogsService.GetDistinctFriendly(ids);
            var distinctCharacters = charanters
                .Select(x => $"{x.Name} {x.Type}")
                .ToList();

            var sb = new StringBuilder();
            sb.AppendLine($"Postacie na {(ids.Length == 1 ? "raidzie" : "raidach")}:".Decorate(Decorator.Underline_bold));
            
            var alginLength = distinctCharacters.Max(x => x.Length);
            foreach (var player in distinctCharacters)
                sb.AppendLine(player.AlginText(alginLength).Decorate(Decorator.Inline_code, true));

            foreach (var msg in sb.ToString().DiscordMessageSplit())
                await Context.Channel.SendMessageAsync(msg);
        }

        [MethodAdmin]
        [Command("podlicz")]
        [Summary("Podliczenie raidu (na podstawie klucza)")]
        public async Task CountFlasks(string fightId, int count)
        {
            var data = await _warcraftLogsService.GetFullFight(fightId);

            if (data != null)
            {
                var fightDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                fightDateTime = fightDateTime.AddMilliseconds(data.Start).ToLocalTime();

                SubstractFlasks(data.Friendlies, count, fightDateTime);
                AddReaction(fightId);

                var counter = 1;
                var sb = new StringBuilder();
                foreach (var friendly in data.Friendlies)
                    sb.AppendLine($"{counter++}. {friendly.Name.Decorate(Decorator.Bold)}  ({friendly.Type.Decorate(Decorator.Italics)})");

                sb.AppendLine(string.Empty);
                sb.AppendLine($"Odejmuje {count.ToString().Decorate(Decorator.Bold)} {count.FlaskGrammaVariety()}");
                
                foreach (var msg in sb.ToString().DiscordMessageSplit())
                    await Context.Channel.SendMessageAsync(msg);

                var nick = _discordContext.GetUserNickname(Context.User.Id);
                await _discordContext.GetChannel(_config.CurrentValue.FlaskChannelId).SendMessageAsync($"Podliczono raid z {fightDateTime.ToString("yyyy-MM-dd".Decorate(Decorator.Bold))} przez użytkownika {nick.Decorate(Decorator.Bold)}");
            }
            else
            {
                await _logger.WriteLog($"Logs {fightId} doesn't exist");
                await Context.Channel.SendMessageAsync($"Logi o Id {fightId} nie istnieją!");
            }
        }

        [MethodAdmin]
        [Command("podlicz")]
        [Summary("Podliczenie raidu (na podstawie reactów)")]
        public async Task CountFlasks(int count)
        {
            var emoteP = new Emoji(_config.CurrentValue.WarcraftLogs.PaidEmaoteCode);
            var emoteV = new Emoji(_config.CurrentValue.WarcraftLogs.DoneEmoteCode);

            var allMessages = await _discordContext.GetChannelMessages(_config.CurrentValue.LogChannelId);
            var messages = allMessages.Where(x => x.Reactions.Keys.Contains(emoteP) && !x.Reactions.Keys.Contains(emoteV)).ToList();

            if (messages.Count == 0)
            {
                await Context.Channel.SendMessageAsync("Nie znaleziono logów do podliczenia!");
                return;
            }
           
            var regEx = new Regex(_config.CurrentValue.WarcraftLogs.FightKeyRegex);
            foreach (var message in messages) {
                var embed = message.Embeds.FirstOrDefault();
                var url = embed == null
                    ? message.Content
                    : embed.Url;

                var a = regEx.Matches(url);
                var key = regEx.Matches(url).Select(x => x.Groups).Select(x => x.Values.Last()).Select(x => x.Value).FirstOrDefault();
                if (!string.IsNullOrEmpty(key))
                    await CountFlasks(key, count);
            }
        }

        private async void SubstractFlasks(List<Friendly> characters, int count, DateTime fightDate)
        {
            var googleSettings = _config.CurrentValue.GoogleSettings;
            var nerdRoster = await _googleService.GetNerdsAsync();
            var curentValues = await _googleService.ReadDataAsync<FlaskModel>(googleSettings.FlaskData.ReportValuesAddres);
            var newMembersLastRow = (await _googleService.ReadCellAsync(googleSettings.NewMembers.ConfigLastRow)).GetInt();

            var newMembers = new List<Friendly>();
            foreach (var character in characters)
            {
                var raider = nerdRoster.FirstOrDefault(x => x.AllNicks.Contains(character.Name, StringComparer.InvariantCultureIgnoreCase));
                if (raider != null)
                {
                    if(raider.Handled) continue; // skip if already counted
                    var flaskData = curentValues.First(x => x.WowNick == raider.MainNick);

                    if (flaskData != null)
                        flaskData.FlaskCount -= count;
                    else
                        newMembers.Add(character);    // missing data

                    raider.Handled = true;
                }
                else
                {
                    newMembers.Add(character);
                }
            }
            
            var rangeToSave = string.Format(googleSettings.FlaskData.ReportFlaskCountAddresTemplate, curentValues.Count + 1);
            var valuesToSave = curentValues.Select(x => x.FlaskCount).ToList();

            var newMembersRangeToSave = string.Format(googleSettings.NewMembers.NewMembersAddresTemplate, newMembersLastRow, newMembersLastRow + newMembers.Count);
            var newMembersValuesToSave = newMembers.Select(x => new {fightDate = fightDate.ToString("yyyy-MM-dd"), x.Name, x.Server, x.Type}).ToList();

            await _googleService.WriteDataAsync(rangeToSave, valuesToSave);
            await _googleService.WriteDataAsync(newMembersRangeToSave, newMembersValuesToSave);
            await _googleService.WriteDataAsync(googleSettings.FlaskData.ReportSubstarctDateAddres, DateTime.Now.ToString(googleSettings.FlaskData.ReportDateFormat));
        }

        private async void AddReaction(string fightId)
        {
            var emote = new Emoji(_config.CurrentValue.WarcraftLogs.DoneEmoteCode);
            var msgs = await _discordContext.GetChannelMessages(_config.CurrentValue.LogChannelId);

            foreach (var msg in msgs.Where(msg => msg.Content.Contains(fightId) || msg.Embeds.Any(x => x.Url.Contains(fightId))))
            {
                await msg.AddReactionAsync(emote, RequestOptions.Default);
                break;
            }
        }
    }
}