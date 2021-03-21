using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nerdomat.Interfaces;
using Nerdomat.Models;
using Nerdomat.Modules;
using Nerdomat.Tools;
using Victoria;
using Victoria.EventArgs;

namespace Nerdomat.Services
{
    public class MusicService : IMusicService
    {
        private readonly LavaNode _lavaSocketClient;
        private readonly LavaConfig _lavaConfig;
        private readonly IOptionsMonitor<Config> _config;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;
        private readonly ILoggerService _logger;

        public MusicService(IServiceProvider services, IOptionsMonitor<Config> config)
        {
            _lavaSocketClient = services.GetRequiredService<LavaNode>();
            _lavaConfig = services.GetRequiredService<LavaConfig>();
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _logger = services.GetRequiredService<ILoggerService>();
            _services = services;
            _config = config;

            Console.WriteLine($"{GetType().Name} initialized");
        }

        public Task InitializeAsync()
        {
            _discord.Ready += ClientReadyAsync;
            //_lavaSocketClient.OnLog += LogAsync;
            _lavaSocketClient.OnTrackEnded += TrackFinished;
            return Task.CompletedTask;
        }

        public async Task ConnectAsync(SocketVoiceChannel voiceChannel, ITextChannel textChannel)
            => await _lavaSocketClient.JoinAsync(voiceChannel, textChannel);

        public async Task LeaveAsync(SocketVoiceChannel voiceChannel)
            => await _lavaSocketClient.LeaveAsync(voiceChannel);

        // public async Task<string> PlayAsync(string query, IGuild guildId)
        // {
        //     var _player = _lavaSocketClient.GetPlayer(guildId);
        //     var results = await SearchSongAsync(query);

        //     if (results.LoadStatus == Victoria.Enums.LoadStatus.NoMatches || results.LoadStatus == Victoria.Enums.LoadStatus.LoadFailed)
        //         return "Nie znaleziono";

        //     var track = results.Tracks.FirstOrDefault();

        //     if (_player.PlayerState == Victoria.Enums.PlayerState.Playing)
        //     {
        //         _player.Queue.Enqueue(track);
        //         return $"Dodano do kolejki: {track.Title.SongTitleTrim()} ({(track.Duration.Hours > 0 ? track.Duration.ToString(@"hh\:mm\:ss") : track.Duration.ToString(@"mm\:ss"))})";
        //     }
        //     else
        //     {
        //         await _player.PlayAsync(track);
        //         return $"Teraz odtwarzane: {track.Title.SongTitleTrim()} ({(track.Duration.Hours > 0 ? track.Duration.ToString(@"hh\:mm\:ss") : track.Duration.ToString(@"mm\:ss"))})";
        //     }
        // }

        public async Task<string> PlayAsync(string query, IGuild guildId)
        {
            var _player = _lavaSocketClient.GetPlayer(guildId);
            var results = await SearchSongAsync(query);

            if (results.LoadStatus == Victoria.Enums.LoadStatus.NoMatches || results.LoadStatus == Victoria.Enums.LoadStatus.LoadFailed)
                return "Nie znaleziono";

            var sb = new StringBuilder();

            foreach (var track in SongsFromSearchGet(results))
            {
                if (_player.PlayerState == Victoria.Enums.PlayerState.Playing)
                {
                    _player.Queue.Enqueue(track);
                    sb.AppendLine($"Dodano do kolejki: {track.Title.SongTitleTrim()} ({(track.Duration.Hours > 0 ? track.Duration.ToString(@"hh\:mm\:ss") : track.Duration.ToString(@"mm\:ss"))})");
                }
                else
                {
                    await _player.PlayAsync(track);
                    sb.AppendLine($"Teraz odtwarzane: {track.Title.SongTitleTrim()} ({(track.Duration.Hours > 0 ? track.Duration.ToString(@"hh\:mm\:ss") : track.Duration.ToString(@"mm\:ss"))})");
                }
            }

            return sb.ToString();
        }

        public async Task<string> StopAsync(IGuild guildId)
        {
            var _player = _lavaSocketClient.GetPlayer(guildId);
            if (_player is null)
                return "Błąd odtwarzacza";
            await _player.StopAsync();
            return "Zatrzymano odtwarzanie";
        }

        public async Task<string> SkipAsync(IGuild guildId)
        {
            var _player = _lavaSocketClient.GetPlayer(guildId);
            if (_player is null || _player.Queue.Count() is 0)
                return "Kolejka jest pusta";

            var oldTrack = _player.Track;
            await _player.SkipAsync();
            return $"Pominięto: {oldTrack.Title.SongTitleTrim()} \nTeraz odtwarzane: {_player.Track.Title.SongTitleTrim()}";
        }

        public async Task<string> SetVolumeAsync(ushort vol, IGuild guildId)
        {
            var _player = _lavaSocketClient.GetPlayer(guildId);
            if (_player is null)
                return "Odtwarzacz nic nie odtwarza";

            if (vol > 150 || vol <= 2)
            {
                return "Proszę użyj liczby z zakresu 2 - 150";
            }

            await _player.UpdateVolumeAsync(vol);
            return $"Głośność ustawiona na: {vol}";
        }

        public async Task<string> PauseOrResumeAsync(IGuild guildId)
        {
            var _player = _lavaSocketClient.GetPlayer(guildId);
            if (_player is null)
                return "Odtwarzacz nic nie odtwarza";

            if (!(_player.PlayerState == Victoria.Enums.PlayerState.Paused))
            {
                await _player.PauseAsync();
                return "Wstrzymano odtwarzanie";
            }
            else
            {
                await _player.ResumeAsync();
                return "Wznowiono odtwarzanie";
            }
        }

        public async Task<string> ResumeAsync(IGuild guildId)
        {
            var _player = _lavaSocketClient.GetPlayer(guildId);
            if (_player is null)
                return "Odtwarzacz nic nie odtwarza";

            if (_player.PlayerState == Victoria.Enums.PlayerState.Paused)
            {
                await _player.ResumeAsync();
                return "Wznowiono odtwarzanie";
            }

            return "Odtwarzacz nie jest zapauzowany";
        }

        public Task<string> QueueGetAsync(IGuild guildId)
        {
            return Task.Factory.StartNew(() =>
            {
                var _player = _lavaSocketClient.GetPlayer(guildId);
                if (_player is null)
                    return "Odtwarzacz nic nie odtwarza";

                if (_player.Track is null)
                    return "Odtwarzacz nic nie odtwarza";

                var counter = 1;
                var sb = new StringBuilder();
                sb.AppendLine("Aktualna kolejka to:".Decorate(Decorator.Underline_bold));
                sb.AppendLine($"\t{counter++}. {_player.Track.Title.SongTitleTrim()}".Decorate(Decorator.Bold_Italics));

                foreach (var track in _player.Queue)
                    sb.AppendLine($"\t{counter++}. {track.Title.SongTitleTrim()}".Decorate(Decorator.Italics));

                return sb.ToString();
            });
        }

        public Task<string> RemoveFromQueueAsync(IGuild guildId, int id)
        {
            return Task.Factory.StartNew(() =>
            {
                var _player = _lavaSocketClient.GetPlayer(guildId);
                if (_player is null)
                    return "Odtwarzacz nic nie odtwarza";

                var track = _player.Queue.ElementAtOrDefault(id - 2);
                if (track is null)
                    return $"Nie znaleziono elementu z indexem {id}";

                _player.Queue.Remove(track);
                return $"Usunięto z kolejki: {track.Title.SongTitleTrim()}";
            });
        }

        public async Task<string> RemoveFromQueueAsync(IGuild guildId, string name)
        {
            var _player = _lavaSocketClient.GetPlayer(guildId);
            var results = await SearchSongAsync(name);

            if (_player is null)
                return "Odtwarzacz nic nie odtwarza";

            if (results.LoadStatus == Victoria.Enums.LoadStatus.NoMatches || results.LoadStatus == Victoria.Enums.LoadStatus.LoadFailed)
                return "Nie znaleziono";

            var track = _player.Queue.FirstOrDefault(x => string.Equals(x.Title, results.Tracks.First().Title, StringComparison.CurrentCultureIgnoreCase));
            if (track is null)
                return $"Nie znaleziono {name} w kolejce";

            _player.Queue.Remove(track);
            return $"Usunięto z kolejki: {track.Title.SongTitleTrim()}";
        }

        private async Task ClientReadyAsync()
        {
            if (!_lavaSocketClient.IsConnected)
            {
                await _lavaSocketClient.ConnectAsync();
            }
        }

        private async Task<Victoria.Responses.Rest.SearchResponse> SearchSongAsync(string query)
        {
            var results = await _lavaSocketClient.SearchAsync(query);

            if (results.LoadStatus == Victoria.Enums.LoadStatus.NoMatches || results.LoadStatus == Victoria.Enums.LoadStatus.LoadFailed)
            {
                results = await _lavaSocketClient.SearchYouTubeAsync(query);
            }

            return results;
        }

        private IEnumerable<LavaTrack> SongsFromSearchGet(Victoria.Responses.Rest.SearchResponse searchResoult)
        {
            if (searchResoult.LoadStatus == Victoria.Enums.LoadStatus.NoMatches || searchResoult.LoadStatus == Victoria.Enums.LoadStatus.LoadFailed)
                yield return null;

            if (searchResoult.LoadStatus == Victoria.Enums.LoadStatus.PlaylistLoaded && !string.IsNullOrWhiteSpace(searchResoult.Playlist.Name))
            {
                foreach (var song in searchResoult.Tracks)
                {
                    yield return song;
                }
            }
            else
            {
                yield return searchResoult.Tracks.FirstOrDefault();
            }
        }

        private async Task TrackFinished(TrackEndedEventArgs args)
        {
            await _logger.WriteLog("TrackFinished triggered");
            if (!args.Reason.ShouldPlayNext())
            {
                return;
            }

            var player = args.Player;
            if (!player.Queue.TryDequeue(out var queueable))
            {
                await player.TextChannel.SendMessageAsync("Kolejka jest już pusta");
                return;
            }

            if (!(queueable is LavaTrack track))
            {
                await player.TextChannel.SendMessageAsync("Z kolejką jest coś nie tak");
                return;
            }

            await args.Player.PlayAsync(track);
            await player.TextChannel.SendMessageAsync($"Teraz odtwarzane: {track.Title.SongTitleTrim()}");
        }

        private async Task LogAsync(LogMessage logMessage)
        {
            await _logger.WriteLog(logMessage.Message);
        }
    }
}