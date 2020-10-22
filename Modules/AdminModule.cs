using System;
using System.IO;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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

        public AdminModule(IServiceProvider services, IOptionsMonitor<Config> config)
        {
            _discord = services.GetRequiredService<DiscordSocketClient>();
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
    }
}