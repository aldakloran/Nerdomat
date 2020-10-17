using System;
using System.Threading.Tasks;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nerdomat.Interfaces;
using Nerdomat.Models;
using Nerdomat.Tools;

namespace Nerdomat.Services
{
    public class LoggerService : ILoggerService
    {
        private readonly IOptionsMonitor<Config> _config;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;
        
        public LoggerService(IServiceProvider services, IOptionsMonitor<Config> config)
        {
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _services = services;
            _config = config;
        }

        public async Task WriteLog(string message)
        {
            if (_discord.GetGuild(_config.CurrentValue.MyGuildId).GetChannel(_config.CurrentValue.LogChannelId) is SocketTextChannel channel)
                await channel.SendMessageAsync(message.Decorate(Decorator.Block_code));

            Console.WriteLine(message);
        }
    }
}