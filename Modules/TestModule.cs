using System;
using System.IO;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nerdomat.Interfaces;
using Nerdomat.Models;
using Nerdomat.Tools;
using Newtonsoft.Json;

namespace Nerdomat.Modules
{
    [ModuleActive(false)]
    [ModuleName("Test")]
    public class TestModule : ModuleBase<SocketCommandContext>
    {
        private readonly IOptionsMonitor<Config> _config;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;
        private readonly IWarcraftLogsService _warcraftLogsService;

        public TestModule(IServiceProvider services, IOptionsMonitor<Config> config)
        {
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _warcraftLogsService = services.GetRequiredService<IWarcraftLogsService>();
            _services = services;
            _config = config;
        }

        [Command("test")]
        [Summary("Kontener na randomowe testy")]
        public async Task Test()
        {
            var players = await _warcraftLogsService.GetCharacters("1J3rRY6L7Ng4zAyv");
            foreach (var player in players)
            {
                await Context.Channel.SendMessageAsync($"{player.Name} {player.Server} {player.Type}");
            }
        }
        
        [Command("json")]
        [Summary("Zapisuje ustawienia do jsona")]
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

            await Context.Channel.SendMessageAsync($"Saved current config to:\n{file}".Decorate(Decorator.Block_code));
        }
    }
}