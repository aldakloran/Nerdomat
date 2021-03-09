using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nerdomat.Interfaces;
using Nerdomat.Models;
using Nerdomat.Tools;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nerdomat.Modules
{
    [ModuleActive(true)]
    [ModuleName("Muzyka (beta)")]
    public class MusicModule : ModuleBase<SocketCommandContext>
    {
        private readonly IOptionsMonitor<Config> _config;
        private readonly IMusicService _music;
        private readonly IServiceProvider _services;
        private readonly ILoggerService _logger;

        public MusicModule(IServiceProvider services, IOptionsMonitor<Config> config)
        {
            _logger = services.GetRequiredService<ILoggerService>();
            _music = services.GetRequiredService<IMusicService>();
            _services = services;
            _config = config;
        }

        [Command("Join")]
        [Summary("Zaproś bota na kanał")]
        public async Task Join()
        {
            var user = Context.User as SocketGuildUser;
            if (user.VoiceChannel is null)
            {
                await ReplyAsync("Musisz być na kanale głosowym");
                return;
            }
            else
            {
                await _music.ConnectAsync(user.VoiceChannel, Context.Channel as ITextChannel);
                await ReplyAsync($"Połączono z kanałem {user.VoiceChannel.Name}");
            }
        }

        [Command("Leave")]
        [Summary("Wyrzuć bota z kanału")]
        public async Task Leave()
        {
            var user = Context.User as SocketGuildUser;
            if (user.VoiceChannel is null)
            {
                await ReplyAsync("Połącz się z kanałem na którym jest bot by go rozłączyć");
            }
            else
            {
                await _music.LeaveAsync(user.VoiceChannel);
                await ReplyAsync($"Rozłączono z kanałem {user.VoiceChannel.Name}");
            }
        }

        [Command("Play")]
        [Summary("Rozpocznij odtwarzanie")]
        public async Task Play([Remainder] string query)
            => await ReplyAsync(await _music.PlayAsync(query, Context.Guild));


        [Command("Stop")]
        [Summary("Zatrzymaj odtwarzanie")]
        public async Task Stop()
            => await ReplyAsync(await _music.StopAsync(Context.Guild));

        [Command("Skip")]
        [Summary("Pomiń")]
        public async Task Skip()
            => await ReplyAsync(await _music.SkipAsync(Context.Guild));

        [Command("Volume")]
        [Summary("Ustaw głośność")]
        public async Task Volume(ushort vol)
            => await ReplyAsync(await _music.SetVolumeAsync(vol, Context.Guild));

        [Command("Pause")]
        [Summary("Wstrzymaj odtwarzanie")]
        public async Task Pause()
            => await ReplyAsync(await _music.PauseOrResumeAsync(Context.Guild));

        [Command("Resume")]
        [Summary("Wznów odtwarzanie")]
        public async Task Resume()
            => await ReplyAsync(await _music.ResumeAsync(Context.Guild));
    }
}
