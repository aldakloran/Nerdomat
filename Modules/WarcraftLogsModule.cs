using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nerdomat.Interfaces;
using Nerdomat.Models;
using Nerdomat.Tools;

namespace Nerdomat.Modules
{
    public class WarcraftLogsModule : ModuleBase<SocketCommandContext>
    {
        private readonly IOptionsMonitor<Config> _config;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;
        private readonly IWarcraftLogsService _warcraftLogsService;
        private readonly ILoggerService _logger;

        public WarcraftLogsModule(IServiceProvider services, IOptionsMonitor<Config> config)
        {
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _warcraftLogsService = services.GetRequiredService<IWarcraftLogsService>();
            _logger = services.GetRequiredService<ILoggerService>();
            _services = services;
            _config = config;
        }

        [Command("wszyscy")]
        [Summary("Wyliczenie wszystkich postaci na raidach")]
        public async Task GetRaiders(params string[] ids)
        {
            var charanters = new List<Friendly>();
            foreach (var fightId in ids)
            {
                await _logger.WriteLog($"Downloading {fightId} fights");
                var players = await _warcraftLogsService.GetCharacters(fightId);
                charanters.AddRange(players);
            }

            var distinctCharacters = charanters.Select(x => new {x.Name, x.Type})
                                                                      .OrderBy(x => x.Name)
                                                                      .ThenBy(x => x.Type)
                                                                      .Select(x => $"{x.Name} {x.Type}")
                                                                      .Distinct()
                                                                      .ToList();

            var sb = new StringBuilder();
            sb.AppendLine($"Postacie na {(ids.Length == 1 ? "raidzie" : "raidach")}:".Decorate(Decorator.Underline_bold));
            
            var alginLength = distinctCharacters.Max(x => x.Length);
            foreach (var player in distinctCharacters)
                sb.AppendLine(player.AlginText(alginLength).Decorate(Decorator.Inline_code, true));

            foreach (var msg in sb.ToString().DiscordMessageSplit())
                await Context.Channel.SendMessageAsync(msg);
        }
    }
}