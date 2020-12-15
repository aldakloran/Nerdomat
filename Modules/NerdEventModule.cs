using System;
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
    [ModuleName("Eventy")]
    public class NerdEventModule : ModuleBase<SocketCommandContext>
    {
        private readonly IOptionsMonitor<Config> _config;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;
        private readonly ILoggerService _logger;
        private readonly IDiscordContextService _discordContext;

        public NerdEventModule(IServiceProvider services, IOptionsMonitor<Config> config)
        {
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _logger = services.GetRequiredService<ILoggerService>();
            _discordContext = services.GetRequiredService<IDiscordContextService>();
            _services = services;
            _config = config;
        }

        [Command("sl")]
        [Summary("Wyświetla czas do premiery")]
        public async Task SLRelease()
        {
            var rlDate = new DateTime(2020, 11, 24, 00, 00, 00);
            var diff = rlDate.Subtract(DateTime.Now);

            var text = diff.TotalSeconds > 0
                ? $"do premiery zostało {diff.Hours}[h] {diff.Minutes}[min] {diff.Seconds}[sec]".Decorate(Decorator.Block_code)
                : "Gra już działa™️";

            await Context.Channel.SendMessageAsync(text);
        }

        [Command("zdjecia")]
        [Summary("Przesyła link do albumu ze zdjęciami")]
        public async Task Pictures()
        {
            var discordCache = "%" + DateTime.Now.ToString("yyyyMMddhhmmss");
            var newEmbed = new EmbedBuilder
            {
                Color = Color.Blue,
                ThumbnailUrl = @"https://lh3.googleusercontent.com/pw/ACtC-3f7aAeiWDaaqoFrc8Sdf6evjSE97hb3VvQkj6PWG1s97z5tG_ZpSzw5b0UPxnq33h_6LjCFfX_KfacydrjPUd6gPdO8PRw2Fpnva2_YG5nnpSLsJVWny--1e3zpPtyaPFTak8_W27ahL3r4j8nJf592=w824-h817-no?authuser=0",//CategoryUrl,//LogoUrl,
                ImageUrl = @"https://lh3.googleusercontent.com/pw/ACtC-3ejbk1__9qi2Gkqkre4jbP3yuWaKBqEUmz00oOnQOzlynXURMuibx5OnJdRGh-rwe9j7y1UKXptuHd4fIg3GOOon6izcN9g8gzCo8veH3rtLuxPJxh8Ex5nZ38Rst8UNrwDFdlamq2cGw42btFVZRZG=w807-h606-no?authuser=0" + discordCache,
                Url = @"https://photos.app.goo.gl/1ELHDWnbHfNUDrZN8",
                Title = $"Zdjęcia nerdów".Decorate(Decorator.Underline_bold)
            };

            await Context.Channel.SendMessageAsync(string.Empty, false, newEmbed.Build());
        }
    }
}