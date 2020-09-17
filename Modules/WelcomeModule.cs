using System;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nerdomat.Models;

namespace Nerdomat.Modules
{
    public class WelcomeModule : ModuleBase
    {
        private readonly IOptionsMonitor<Config> _config;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;

        public WelcomeModule(IServiceProvider services, IOptionsMonitor<Config> config)
        {
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _services = services;
            _config = config;

            _discord.UserJoined += UserJoinedEvent;
        }

        private async Task UserJoinedEvent(SocketGuildUser arg)
        {
            const string msg = "test";
            await arg.SendMessageAsync(msg);
        }
    }
}