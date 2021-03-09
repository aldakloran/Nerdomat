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

        [Command("BlizzCon")]
        [Summary("Podaje timery BlizzConu")]
        public async Task Blizzcon()
        {
            var blizzconDate = new DateTime(2021, 02, 19);
            var diff = blizzconDate.Subtract(DateTime.Now);

            var container = new EmbedFieldBuilder { IsInline = true, Name = "BlizzCon timer:" };
            if (blizzconDate.Date == DateTime.Now.Date)
            {
                // blizzcon in proggres
                container.Value = $"BlizzCon już dziś!";
            }
            else if (blizzconDate.Date < DateTime.Now.Date)
            {
                // blizzcon passed
                container.Value = $"BlizzCon już był!";
            }
            else
            {
                // blizzcon incomming
                container.Value = $"Dni: {((int)diff.TotalDays).ToString().Decorate(Decorator.Bold)}\nGodzin: {((int)diff.TotalHours).ToString().Decorate(Decorator.Bold)}\nMinut: {((int)diff.TotalMinutes).ToString().Decorate(Decorator.Bold)}\n Sekund: {((int)diff.TotalSeconds).ToString().Decorate(Decorator.Bold)}";
            }

            var emb = new EmbedBuilder
            {
                Color = Color.Blue,
                ThumbnailUrl = @"https://upload.wikimedia.org/wikipedia/commons/thumb/b/b2/Blizzard_Entertainment_Logo.svg/1200px-Blizzard_Entertainment_Logo.svg.png",//CategoryUrl,//LogoUrl,
                ImageUrl = @"https://bnetcmsus-a.akamaihd.net/cms/blog_header/18/18YEXZEX1K871600388208960.jpg",
                Url = @"https://blizzcon.com/en-us/news/23513935/save-the-date-for-blizzconline-february-19-20",
                Title = $"BlizzCon {blizzconDate.Year}"
            };

            var footer = new EmbedFooterBuilder
            {
                Text = $"[{DateTime.Now:yyyy-MM-dd  HH:mm:ss}]"
            };

            emb.Footer = footer;
            emb.AddField(container);

            await Context.Channel.SendMessageAsync(string.Empty, false, emb.Build());
        }
    }
}