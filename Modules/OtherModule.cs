using System;
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
    [ModuleActive(true)]
    [ModuleName("Inne")]
    public class OtherModule : ModuleBase<SocketCommandContext>
    {
        private readonly IOptionsMonitor<Config> _config;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;
        private readonly IWarcraftLogsService _warcraftLogsService;

        public OtherModule(IServiceProvider services, IOptionsMonitor<Config> config)
        {
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _warcraftLogsService = services.GetRequiredService<IWarcraftLogsService>();
            _services = services;
            _config = config;
        }

        [Command("roll")]
        [Summary("Losuje liczbę od 0 do max (domyślnie 100)")]
        public async Task Roll(int max = 100)
        {
            var rng = new Random();
            await ReplyAsync($"{rng.Next(0, Math.Abs(max))} (0-{Math.Abs(max)})");
        }

        [Command("ping")]
        [Summary("Podaje latency do servera discorda")]
        public async Task Ping()
            => await ReplyAsync($"Latency: ({_discord?.Latency ?? 0}ms)".Decorate(Decorator.Block_code));
    }
}