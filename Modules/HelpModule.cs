using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using Nerdomat.Models;
using Nerdomat.Tools;

namespace Nerdomat.Modules
{
    [ModuleActive(false)]
    [ModuleName("Pomoc")]
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        // Dependency Injection will fill this value in for us
        private readonly IOptionsMonitor<Config> _config;

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