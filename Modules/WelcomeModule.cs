using System;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace Nerdomat.Modules
{
    public class WelcomeModule : ModuleBase
    {
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;

        public WelcomeModule(IServiceProvider services)
        {
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _services = services;

            _discord.UserJoined += UserJoinedEvent;
        }

        private async Task UserJoinedEvent(SocketGuildUser arg)
        {
            const string msg = "test";
            await arg.SendMessageAsync(msg);
        }
    }
}