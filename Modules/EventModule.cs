using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nerdomat.Interfaces;
using Nerdomat.Models;
using Nerdomat.Tools;
using Victoria;

namespace Nerdomat.Modules
{
    public class EventHandlerModule : ModuleBase
    {
        private readonly IOptionsMonitor<Config> _config;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;
        private readonly IDiscordContextService _discordContext;
        private readonly IGoogleService _googleService;
        private readonly ILoggerService _logger;
        private readonly LavaNode _lavaSocketClient;
        private readonly IMusicService _music;

        public EventHandlerModule(IServiceProvider services, IOptionsMonitor<Config> config)
        {
            _music = services.GetRequiredService<IMusicService>();
            _lavaSocketClient = services.GetRequiredService<LavaNode>();
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _discordContext = services.GetRequiredService<IDiscordContextService>();
            _googleService = services.GetRequiredService<IGoogleService>();
            _logger = services.GetRequiredService<ILoggerService>();
            _services = services;
            _config = config;

            InitializeEvents();
        }

        private void InitializeEvents()
        {
            _discord.Ready -= DiscordReady;
            _discord.UserJoined -= UserJoinServer;
            _discord.UserLeft -= UserLeftServer;
            _discord.UserBanned -= UserBaned;
            _discord.UserVoiceStateUpdated -= UserVoiceUpdate;

            _discord.Ready += DiscordReady;
            _discord.UserJoined += UserJoinServer;
            _discord.UserLeft += UserLeftServer;
            _discord.UserBanned += UserBaned;
            _discord.UserVoiceStateUpdated += UserVoiceUpdate;

            Console.WriteLine($"{GetType().Name} initialized");
        }

        private async Task DiscordReady()
        {
            var role = _discord.GetGuild(_config.CurrentValue.MyGuildId).GetRole(_config.CurrentValue.DefaultUserRole);
            var users = _discord.GetGuild(_config.CurrentValue.MyGuildId).Users.Where(x => !x.Roles.Contains(role));

            foreach (var usserToAssign in users)
                await usserToAssign.AddRoleAsync(role);
        }


        public async Task UserJoinServer(SocketGuildUser arg)
        {
            var guildData = _config.CurrentValue.GuildData;
            var raidLeader = _discordContext.GetUserNickname(guildData.GuildRaidLeadreId);

            var raidLeaderName = string.Equals(raidLeader, guildData.GuildRaidLeaderName, StringComparison.OrdinalIgnoreCase)
                ? raidLeader
                : $"{raidLeader} ({guildData.GuildRaidLeaderName})";

            var defaultRole = _discord.GetGuild(_config.CurrentValue.MyGuildId)
                                      .GetRole(_config.CurrentValue.DefaultUserRole);

            var sb = new StringBuilder();
            sb.AppendLine($"Witaj na Discordzie gildi {guildData.GuildName.Decorate(Decorator.Bold)}");
            sb.AppendLine("Jak chcesz dowiedzieć się więcej zapraszamy na stronę gildii lub do kontaktu z Oficerami");
            sb.AppendLine(guildData.GuildUrl);
            sb.AppendLine(string.Empty);
            sb.AppendLine("Do mówienia na kanale raidowym wymagane jest Push-to-talk".Decorate(Decorator.Underline));
            sb.AppendLine("W przypadku jakichkolwiek pytań zachęcamy do kontaktu :)");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"W razie pytań odnośnie raidów zachęcam do kontaktu z Raid Leaderem - {raidLeaderName}");
            sb.AppendLine(string.Empty);
            sb.AppendLine(string.Empty);
            sb.AppendLine($"{"Jeżeli chcesz poznać funkcję bota zapraszam do wykorzystania komendy".Decorate(Decorator.Italics)} {"!pomoc".Decorate(Decorator.Bold)}");

            await arg.AddRoleAsync(defaultRole);
            await arg.SendMessageAsync(sb.ToString());
        }

        public async Task UserLeftServer(SocketGuildUser arg)
        {
            var leaveDate = DateTime.Now;
            var role = string.Empty;
            foreach (var rola in arg.Roles.Where(x => x.Name != "@everyone"))
            {
                role += string.IsNullOrEmpty(role)
                    ? rola.Name
                    : $"; {rola.Name}";
            }

            role = string.IsNullOrEmpty(role) ? "---" : role.Replace("@", string.Empty);

            var sb = new StringBuilder();
            sb.AppendLine($"Użytkownik opuścił serwer:".Decorate(Decorator.Underline_bold));
            sb.AppendLine(string.Empty);
            sb.AppendLine("\t" + "Discord: ".Decorate(Decorator.Bold) + $"{arg.Username}#{arg.DiscriminatorValue}".Decorate(Decorator.Italics));
            sb.AppendLine("\t" + "Nick: ".Decorate(Decorator.Bold) + arg.Nickname.Decorate(Decorator.Italics));
            sb.AppendLine("\t" + "Role: ".Decorate(Decorator.Bold) + role.Decorate(Decorator.Italics));
            sb.AppendLine("\t" + "Dołączył: ".Decorate(Decorator.Bold) + (arg.JoinedAt?.ToString("yyyy-MM-dd") ?? "brak danych").Decorate(Decorator.Italics));

            var auditLogsData = await arg.Guild.GetAuditLogsAsync(100).Flatten().Where(x => x.Action == ActionType.Kick || x.Action == ActionType.Ban).ToListAsync();
            var kicked = auditLogsData.FirstOrDefault(x =>
            {
                var auditDate = (DateTimeOffset)x.CreatedAt;
                var cDate = auditDate.LocalDateTime;
                var bTime = cDate.Year == leaveDate.Year && cDate.Month == leaveDate.Month && cDate.Day == leaveDate.Day && cDate.Hour == leaveDate.Hour && Math.Abs(cDate.Minute - leaveDate.Minute) <= 1;

                switch (x.Action)
                {
                    case ActionType.Kick:
                        var auditDataKick = (Discord.Rest.KickAuditLogData)x.Data;
                        var b1 = auditDataKick.Target.Id == arg.Id;

                        return b1 && bTime;
                    case ActionType.Ban:
                        var auditDataBan = (Discord.Rest.BanAuditLogData)x.Data;
                        var b2 = auditDataBan.Target.Id == arg.Id;

                        return b2 && bTime;
                    default:
                        return false;
                }
            });

            if (kicked != null)
            {
                var guildUser = _discord.GetGuild(_config.CurrentValue.MyGuildId).GetUser(kicked.User.Id);
                var userName = _discordContext.GetUserNickname(kicked.User.Id);

                var kickBan = kicked.Action == ActionType.Ban
                    ? "zbanowany"
                    : "wyrzucony";

                sb.AppendLine(string.Empty);
                sb.AppendLine();
                sb.AppendLine($"Użytkownik został {kickBan} przez {userName}".Decorate(Decorator.Bold));
            }

            var adminChannel = _discord.GetChannel(_config.CurrentValue.AdminChannelId) as SocketTextChannel;
            var curentValues = await _googleService.ReadDataAsync<FlaskModel>(_config.CurrentValue.GoogleSettings.FlaskData.ReportValuesAddres);

            await adminChannel.SendMessageAsync(sb.ToString(), embed: NerdChecker.GetNerdArmory(curentValues.FirstOrDefault(x => x.DiscordId == arg.Id)));
        }

        public async Task UserBaned(SocketUser arg1, SocketGuild arg2)
        {
            var channel = _discord.GetChannel(_config.CurrentValue.GeneralChannelId) as SocketTextChannel;
            await channel.SendMessageAsync($"{arg1.Mention} został!");
        }

        public async Task UserVoiceUpdate(SocketUser user, SocketVoiceState before, SocketVoiceState after)
        {
            var userName = _discordContext.GetUserNickname(user.Id);
            var sb = new StringBuilder();

            sb.Append($"[{DateTime.Now} - {this.GetType().Name}]: ");
            if (before.VoiceChannel == null && after.VoiceChannel != null)
            {
                sb.AppendLine($"{userName} has join channel:");
                sb.AppendLine($"\t#{after.VoiceChannel.Name}");
            }
            else if (before.VoiceChannel != null && after.VoiceChannel == null)
            {
                sb.AppendLine($"{userName} has left channel:");
                sb.AppendLine($"\t#{before.VoiceChannel.Name}");
            }
            else
            {
                if (string.Equals(before.VoiceChannel.Name, after.VoiceChannel.Name, StringComparison.OrdinalIgnoreCase))
                    return;

                sb.AppendLine($"{userName} has switched channel:");
                sb.AppendLine($"\tfrom: #{before.VoiceChannel.Name}");
                sb.AppendLine($"\tto: #{after.VoiceChannel.Name}");
            }

            if (before.VoiceChannel != null)
                await MusicPlayerAutoLeave(before.VoiceChannel);

            await _logger.WriteLog(sb.ToString());
        }

        private async Task MusicPlayerAutoLeave(SocketVoiceChannel channel)
        {
            var guild = _discord.GetGuild(_config.CurrentValue.MyGuildId);
            if (_lavaSocketClient.TryGetPlayer(guild, out var player))
            {
                if (player.VoiceChannel.Id == channel.Id && !channel.Users.Any(x => !x.IsBot))
                {
                    var textChannel = player.TextChannel;

                    await textChannel.SendMessageAsync($"[Auto] {await _music.StopAsync(guild)}");
                    await _music.LeaveAsync(channel);
                    await textChannel.SendMessageAsync($"[Auto] Rozłączono z kanałem {channel.Name}");
                }
            }
        }
    }
}