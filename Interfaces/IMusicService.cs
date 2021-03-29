using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;
using Victoria;

namespace Nerdomat.Interfaces
{
    public interface IMusicService
    {
        Task InitializeAsync();
        Task ConnectAsync(SocketVoiceChannel voiceChannel, ITextChannel textChannel);
        Task LeaveAsync(SocketVoiceChannel voiceChannel);
        Task<string> PlayAsync(string query, IGuild guildId);
        Task<string> StopAsync(IGuild guildId);
        Task<string> SkipAsync(IGuild guildId);
        Task<string> ShuffleAsync(IGuild guildId);
        Task<string> SetVolumeAsync(ushort vol, IGuild guildId);
        Task<string> PauseOrResumeAsync(IGuild guildId);
        Task<string> ResumeAsync(IGuild guildId);
        Task<string> QueueGetAsync(IGuild guildId);
        Task<string> RemoveFromQueueAsync(IGuild guildId, int id);
        Task<string> RemoveFromQueueAsync(IGuild guildId, string name);
    }
}