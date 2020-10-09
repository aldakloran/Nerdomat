using System;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nerdomat.Models;
using Nerdomat.Services;
namespace Nerdomat.Modules
{
    public class FlaskModule : ModuleBase<SocketCommandContext>
    {
        private readonly IOptionsMonitor<Config> _config;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;
        private readonly GoogleService _googleService;

        public FlaskModule(IServiceProvider services, IOptionsMonitor<Config> config)
        {
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _googleService = services.GetRequiredService<GoogleService>();
            _services = services;
            _config = config;
        }
    }
}