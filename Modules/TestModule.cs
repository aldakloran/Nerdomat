using System;
using System.IO;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nerdomat.Interfaces;
using Nerdomat.Models;
using Nerdomat.Tools;
using Newtonsoft.Json;

namespace Nerdomat.Modules
{
    [ModuleActive(false)]
    [ModuleName("Test")]
    public class TestModule : ModuleBase<SocketCommandContext>
    {
        private readonly IOptionsMonitor<Config> _config;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;
        private readonly IWarcraftLogsService _warcraftLogsService;

        public TestModule(IServiceProvider services, IOptionsMonitor<Config> config)
        {
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _warcraftLogsService = services.GetRequiredService<IWarcraftLogsService>();
            _services = services;
            _config = config;
        }

        [Command("ping")]
        [Summary("Podaje latency do servera discorda")]
        public async Task Ping()
            => await ReplyAsync($"Latency: ({_discord?.Latency ?? 0}ms)".Decorate(Decorator.Block_code));

        [Command("test")]
        [Summary("Kontener na randomowe testy")]
        public async Task Test()
        {
            var players = await _warcraftLogsService.GetFullFight("1J3rRY6L7Ng4zAyv");
            foreach (var player in players.Friendlies)
            {
                await Context.Channel.SendMessageAsync($"{player.Name} {player.Server} {player.Type}");
            }
        }
    }
}