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
using Nerdomat.Modules;
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
            _lavaSocketClient.OnLog += LogAsync;
            _lavaSocketClient.OnTrackEnded += TrackFinished;
            return Task.CompletedTask;
        }

        public async Task ConnectAsync(SocketVoiceChannel voiceChannel, ITextChannel textChannel)
            => await _lavaSocketClient.JoinAsync(voiceChannel, textChannel);

        public async Task LeaveAsync(SocketVoiceChannel voiceChannel)
            => await _lavaSocketClient.LeaveAsync(voiceChannel);

        public async Task<string> PlayAsync(string query, IGuild guildId)
        {
            var _player = _lavaSocketClient.GetPlayer(guildId);
            var results = await _lavaSocketClient.SearchAsync(query);

            if (results.LoadStatus == Victoria.Enums.LoadStatus.NoMatches || results.LoadStatus == Victoria.Enums.LoadStatus.LoadFailed)
            {
                return "Nie znaleziono";
            }

            var track = results.Tracks.FirstOrDefault();

            if (_player.PlayerState == Victoria.Enums.PlayerState.Playing)
            {
                _player.Queue.Enqueue(track);
                return $"{track.Title} dodano do kolejki";
            }
            else
            {
                await _player.PlayAsync(track);
                return $"Teraz odtwarzane: {track.Title}";
            }
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
            return $"Pominięto: {oldTrack.Title} \nTeraz odtwarzane: {_player.Track.Title}";
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


        private async Task ClientReadyAsync()
        {
            if (!_lavaSocketClient.IsConnected)
            {
                await _lavaSocketClient.ConnectAsync();
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
        }

        private async Task LogAsync(LogMessage logMessage)
        {
            await _logger.WriteLog(logMessage.Message);
        }
    }
}