using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using static Nerdomat.Tools.Atributes;

namespace Nerdomat.Modules
{
    [ModuleActive(true)]
    [ModuleName("Narzędzia admina")]
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        [ModuleAdmin]
        [Command("cleanreactsf"), Alias("cleanf"), Summary("Czyści reacty")]
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
    }
}