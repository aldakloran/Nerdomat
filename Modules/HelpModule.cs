using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nerdomat.Models;
using Nerdomat.Tools;

namespace Nerdomat.Modules
{
    [ModuleActive(false)]
    [ModuleName("Pomoc")]
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        private readonly IOptionsMonitor<Config> _config;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;

        public HelpModule(IServiceProvider services, IOptionsMonitor<Config> config)
        {
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _services = services;
            _config = config;
        }

        [Command("pomoc")]
        [Summary("Pomoc - wszystkie komendy")]
        public async Task Help()
        {
            var user = Context.User as SocketGuildUser;

            var msgFull = CommandsCrawler.GetCommandsList(user.IsAdmin());

            await Context.Message.DeleteAsync();
            foreach (var msg in msgFull.DiscordMessageSplit())
                await user.SendMessageAsync(msg);

        }
    }
}