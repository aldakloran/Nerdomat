using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nerdomat.Interfaces;
using Nerdomat.Models;
using Nerdomat.Tools;
using Newtonsoft.Json;

namespace Nerdomat.Modules
{
    [ModuleActive(true)]
    [ModuleName("Narzędzia administracyjne")]
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        private readonly IOptionsMonitor<Config> _config;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;
        private readonly IGoogleService _googleService;


        public AdminModule(IServiceProvider services, IOptionsMonitor<Config> config)
        {
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _googleService = services.GetRequiredService<IGoogleService>();
            _services = services;
            _config = config;
        }

        [MethodAdmin]
        [Command("cleanreactsf")]
        [Alias("cleanf")]
        [Summary("Czyści reacty")]
        public async Task CleanUsersReacts(ulong channel, params ulong[] masIds)
        {
            var chan = (SocketTextChannel)Context.Client.GetChannel(channel);
            foreach (var o in masIds)
            {
                var msg = await chan.GetMessageAsync(o);
                if (msg == null)
                {
                    await Context.Channel.SendMessageAsync($"Nie znalazłem wiadomości {o}");
                    continue;
                }

                var message = (RestUserMessage)msg;
                await message.RemoveAllReactionsAsync();
                await Context.Channel.SendMessageAsync($"{o} Gotowe!");
            }
        }
        
        [MethodAdmin]
        [Command("json")]
        [Summary("Zapisuje ustawienia bota do jsona")]
        public async Task SaveConfigToJson()
        {
            var js = new JsonSerializer
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Include
            };
            
            //Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
            var file = Path.Combine( Directory.GetCurrentDirectory(), @"Config.json");
            if(File.Exists(file))
                File.Delete(file);

            await using (var sw = new StreamWriter(file))
            using (var writer = new JsonTextWriter(sw))
            {
                js.Serialize(writer, _config.CurrentValue);
            }

            await Context.Channel.SendFileAsync(file, "Aktualna konfiguracja:".Decorate(Decorator.Block_code));
            File.Delete(file);
        }

        [MethodAdmin]
        [Command("nerd")]
        [Summary("Zwraca listę zarejestrowanych nerdów")]
        public async Task AllRegisteredNerds()
        {
            var nerds = await _googleService.GetNerdsAsync();
            var sb = new StringBuilder();
            sb.AppendLine("Zarejestrowani nerdzi to:".Decorate(Decorator.Underline_bold));

            var counter = 1;
            foreach (var nerd in nerds.OrderBy(x => x.MainNick))
            {
                var msg = $"{counter++.ToString().Decorate(Decorator.Bold)} {nerd.DiscordTag.Decorate(Decorator.Underline)}  ";
                msg = nerd.AllNicks.Aggregate(msg, (current, nick) => current + (nick.Decorate(Decorator.Inline_code) + " "));

                sb.AppendLine(msg);
            }

            foreach (var msg in sb.ToString().DiscordMessageSplit())
                await Context.Channel.SendMessageAsync(msg);
        }
    }
}