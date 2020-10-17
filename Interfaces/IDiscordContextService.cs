using Discord.WebSocket;

namespace Nerdomat.Interfaces
{
    public interface IDiscordContextService
    {
        string MentionTag(string discordTag);
        ulong GetUserIdFromTag(string discordTag);
        SocketGuildUser GetUserFromTag(string discordTag);
    }
}