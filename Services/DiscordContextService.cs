using System;
using System.Linq;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nerdomat.Interfaces;
using Nerdomat.Models;

namespace Nerdomat.Services
{
    public class DiscordContextService : IDiscordContextService
    {
        private readonly IOptionsMonitor<Config> _config;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;
        
        public DiscordContextService(IServiceProvider services, IOptionsMonitor<Config> config)
        {
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _services = services;
            _config = config;
        }

        public string MentionTag(string discordTag)
        {
            var user = _discord.GetGuild(_config.CurrentValue.MyGuildId).Users
                .Select(x => new {user = x, tag = $"{x.Username}#{x.Discriminator}"})
                .FirstOrDefault(x => string.Equals(x.tag, discordTag, StringComparison.OrdinalIgnoreCase));

            return user?.user.Mention ?? string.Empty;
        }

        public ulong GetUserIdFromTag(string discordTag)
        {
            var user = _discord.GetGuild(_config.CurrentValue.MyGuildId).Users
                .Select(x => new {user = x, tag = $"{x.Username}#{x.Discriminator}"})
                .FirstOrDefault(x => string.Equals(x.tag, discordTag, StringComparison.OrdinalIgnoreCase));

            return user?.user.Id ?? 0ul;
        }

        public SocketGuildUser GetUserFromTag(string discordTag)
        {
            var user = _discord.GetGuild(_config.CurrentValue.MyGuildId).Users
                .Select(x => new {user = x, tag = $"{x.Username}#{x.Discriminator}"})
                .FirstOrDefault(x => string.Equals(x.tag, discordTag, StringComparison.OrdinalIgnoreCase));

            return user?.user;
        }
    }
}