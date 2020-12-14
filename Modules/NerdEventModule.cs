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
    [ModuleName("Eventy")]
    public class NerdEventModule : ModuleBase<SocketCommandContext>
    {
        private readonly IOptionsMonitor<Config> _config;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;
        private readonly ILoggerService _logger;
        private readonly IDiscordContextService _discordContext;

        public NerdEventModule(IServiceProvider services, IOptionsMonitor<Config> config)
        {
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _logger = services.GetRequiredService<ILoggerService>();
            _discordContext = services.GetRequiredService<IDiscordContextService>();
            _services = services;
            _config = config;
        }

        [Command("sl")]
        [Summary("Wyświetla czas do premiery")]
        public async Task SLRelease()
        {
            var rlDate = new DateTime(2020, 11, 24, 00, 00, 00);
            var diff = rlDate.Subtract(DateTime.Now);

            var text = diff.TotalSeconds > 0
                ? $"do premiery zostało {diff.Hours}[h] {diff.Minutes}[min] {diff.Seconds}[sec]".Decorate(Decorator.Block_code)
                : "Gra już działa™️";

            await Context.Channel.SendMessageAsync(text);
        }
    }
}