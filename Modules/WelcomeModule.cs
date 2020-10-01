using System;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nerdomat.Models;
using Nerdomat.Tools;

namespace Nerdomat.Modules
{
    public class WelcomeModule : ModuleBase
    {
        private readonly IOptionsMonitor<Config> _config;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;

        public WelcomeModule(IServiceProvider services, IOptionsMonitor<Config> config)
        {
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _services = services;
            _config = config;

            _discord.UserJoined += UserJoinedEvent;
        }

        private async Task UserJoinedEvent(SocketGuildUser arg)
        {
            var sb = new StringBuilder();
            
            sb.AppendLine($"Witaj na Discordzie gildi {"N E R D".Decorate(Decorator.Underline)}".Decorate(Decorator.Bold));
            sb.AppendLine("https://www.gildia-nerd.pl/");
            sb.AppendLine("lub do kontaktu z Oficerami");
            sb.AppendLine(string.Empty);
            sb.AppendLine("Do mówienia na kanale raidowym wymagane jest Push-to-talk".Decorate(Decorator.Underline));
            sb.AppendLine("W przypadku jakichkolwiek pytań zachęcamy do kontaktu :)");
            sb.AppendLine(string.Empty);
            sb.AppendLine(string.Empty);
            sb.AppendLine($"{"Jeżeli chcesz poznać funkcję bota zapraszam do wykorzystania komendy".Decorate(Decorator.Italics)} {"!Pomoc".Decorate(Decorator.Bold)}");

            var role = _discord.GetGuild(_config.CurrentValue.MyGuildId).GetRole(_config.CurrentValue.DefaultUserRole);
            
            await arg.AddRoleAsync(role);
            await arg.SendMessageAsync(sb.ToString());
        }
    }
}