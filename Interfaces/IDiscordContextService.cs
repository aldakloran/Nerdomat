using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Rest;
using Discord.WebSocket;

namespace Nerdomat.Interfaces
{
    public interface IDiscordContextService
    {
        string MentionTag(string discordTag);
        ulong GetUserIdFromTag(string discordTag);
        SocketGuildUser GetUserFromTag(string discordTag);
        SocketTextChannel GetChannel(ulong channelId);
        Task<RestUserMessage> GetChannelMessage(ulong channelId, ulong messageId);
        Task<List<RestUserMessage>> GetChannelMessages(ulong channelId);
        string GetUserNickname(ulong userId);
    }
}