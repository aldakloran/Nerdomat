using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nerdomat.Interfaces;
using Nerdomat.Models;
using Nerdomat.Tools;

namespace Nerdomat.Modules
{
    [ModuleActive(true)]
    [ModuleName("Raidbots")]
    public class RaidbotsModule : ModuleBase<SocketCommandContext>
    {
        private readonly IOptionsMonitor<Config> _config;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;
        private readonly ILoggerService _logger;
        private readonly IDiscordContextService _discordContext;
        private readonly IGoogleService _googleService;

        public RaidbotsModule(IServiceProvider services, IOptionsMonitor<Config> config)
        {
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _googleService = services.GetRequiredService<IGoogleService>();
            _logger = services.GetRequiredService<ILoggerService>();
            _discordContext = services.GetRequiredService<IDiscordContextService>();
            _services = services;
            _config = config;
        }

        [MethodAdmin]
        [Command("Login")]
        [Summary("ustawia login dla raidbotsów")]
        public async Task SetLogin(string login)
        {
            var loginCell = _config.CurrentValue.RaidbotsAccountData.LoginCellAddress;

            await _googleService.WriteDataAsync(loginCell, login);
            await Context.Channel.SendMessageAsync("Pomyślnie zaktualizowano login dla serwisu Raidbots");
            await Context.Message.DeleteAsync();
        }

        [MethodAdmin]
        [Command("Haslo")]
        [Summary("ustawia haslo dla raidbotsów")]
        public async Task SetPassword(string password)
        {
            var passwordCell = _config.CurrentValue.RaidbotsAccountData.PasswordCellAddress;

            await _googleService.WriteDataAsync(passwordCell, password);
            await Context.Channel.SendMessageAsync("Pomyślnie zaktualizowano hasło dla serwisu Raidbots");
            await Context.Message.DeleteAsync();
        }

        [MethodAdmin]
        [Command("Dostep-wyslij")]
        [Summary("Wysyła wszystkim nerdom nowe hasło")]
        public async Task SendPassToAll()
        {
            var loginCell = _config.CurrentValue.RaidbotsAccountData.LoginCellAddress;
            var passwordCell = _config.CurrentValue.RaidbotsAccountData.PasswordCellAddress;
            var allowedRole = _config.CurrentValue.RaidbotsAccountData.AllowerRoleId;

            var login = await _googleService.ReadCellAsync(loginCell);
            var password = await _googleService.ReadCellAsync(passwordCell);

            foreach (var usr in _discord.GetGuild(_config.CurrentValue.MyGuildId).Users.Where(x => x.Roles.Any(y => y.Id == allowedRole)))
            {

                var container = new EmbedFieldBuilder
                {
                    IsInline = true,
                    Name = "Dane logowania:",
                    Value = $"{("Login: ".Decorate(Decorator.Bold))}{login.Decorate(Decorator.Inline_code)}\n{("Hasło: ".Decorate(Decorator.Bold))}{password.Decorate(Decorator.Inline_code)}"
                };

                var emb = new EmbedBuilder
                {
                    Color = Color.Blue,
                    Url = @"https://www.raidbots.com/auth",
                    Title = $"Konto do raidbotsów"
                };

                var footer = new EmbedFooterBuilder
                {
                    Text = $"[{DateTime.Now:yyyy-MM-dd  HH:mm:ss}]"
                };

                emb.Footer = footer;
                emb.AddField(container);

                await usr.SendMessageAsync(string.Empty, false, emb.Build());
                await Context.Channel.SendMessageAsync($"Wysłano wiadomość z danymi dostępowymi do: {_discordContext.GetUserNickname(usr.Id)}");
            }
        }

        [Command("Dostep")]
        [Summary("przesyła dane konta do raidbotsów")]
        public async Task RaidbotsAccountGet()
        {
            var loginCell = _config.CurrentValue.RaidbotsAccountData.LoginCellAddress;
            var passwordCell = _config.CurrentValue.RaidbotsAccountData.PasswordCellAddress;
            var allowedRole = _config.CurrentValue.RaidbotsAccountData.AllowerRoleId;

            await Context.Message.DeleteAsync();
            if (_discordContext.UserInRole(Context.User.Id, allowedRole))
            {
                var login = await _googleService.ReadCellAsync(loginCell);
                var password = await _googleService.ReadCellAsync(passwordCell);

                var container = new EmbedFieldBuilder
                {
                    IsInline = true,
                    Name = "Dane logowania:",
                    Value = $"{("Login: ".Decorate(Decorator.Bold))}{login.Decorate(Decorator.Inline_code)}\n{("Hasło: ".Decorate(Decorator.Bold))}{password.Decorate(Decorator.Inline_code)}"
                };

                var emb = new EmbedBuilder
                {
                    Color = Color.Blue,
                    Url = @"https://www.raidbots.com/auth",
                    Title = $"Konto do raidbotsów"
                };

                var footer = new EmbedFooterBuilder
                {
                    Text = $"[{DateTime.Now:yyyy-MM-dd  HH:mm:ss}]"
                };

                emb.Footer = footer;
                emb.AddField(container);

                await Context.User.SendMessageAsync(string.Empty, false, emb.Build());
            }
            else
            {
                await Context.User.SendMessageAsync("Nie masz odpowiednich uprawnień by uzyskać dostęp do konta, skontaktuj się z oficerami");
            }
        }
    }
}