using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Rest;
using Discord.WebSocket;

namespace Nerdomat.Interfaces
{
    public interface IDiscordContextService
    {
        bool UserInRole(ulong discordId, ulong role);
        string MentionTag(string discordTag);
        string MentionId(ulong discordId);
        ulong GetUserIdFromTag(string discordTag);
        SocketGuildUser GetUserFromTag(string discordTag);
        SocketTextChannel GetChannel(ulong channelId);
        Task<RestUserMessage> GetChannelMessage(ulong channelId, ulong messageId);
        Task<List<RestUserMessage>> GetChannelMessages(ulong channelId);
        string GetUserNickname(ulong userId);
    }
}