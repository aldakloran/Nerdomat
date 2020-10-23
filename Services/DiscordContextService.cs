using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Rest;
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

        public SocketTextChannel GetChannel(ulong channelId)
        {
            var channel = _discord.GetGuild(_config.CurrentValue.MyGuildId).Channels
                .FirstOrDefault(x => x.Id == channelId);

            return channel as SocketTextChannel;
        }

        public async Task<RestUserMessage> GetChannelMessage(ulong channelId, ulong messageId)
        {
            var channel = GetChannel(channelId);
            return await channel.GetMessageAsync(messageId) as RestUserMessage;
        }

        public async Task<List<RestUserMessage>> GetChannelMessages(ulong channelId)
        {
            var channel = GetChannel(channelId);
            var msgs = await channel.GetMessagesAsync().Flatten().Select(x => x as RestUserMessage).ToListAsync();

            return msgs;
        }

        public string GetUserNickname(ulong userId)
        {
            var user = _discord.GetGuild(_config.CurrentValue.MyGuildId).GetUser(userId);
            return string.IsNullOrEmpty(user.Nickname)
                ? user.Username
                : user.Nickname;
        }
    }
}