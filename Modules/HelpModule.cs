using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Nerdomat.Tools;

namespace Nerdomat.Modules
{
    [ModuleActive(false)]
    [ModuleName("Pomoc")]
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        [Command("pomoc")]
        [Summary("Pomoc - wszystkie komendy")]
        public async Task Help()
        {
            var user = Context.User as SocketGuildUser;

            var msgFull = CommandsCrawler.GetCommandsList(user.IsAdmin());
            var msgList = new List<string>();
            if (msgFull.Length >= 2000)
            {
                var sb = new StringBuilder();
                foreach (var sentence in Regex.Split(msgFull, Environment.NewLine))   // jebany linux
                {
                    if (sb.Length + sentence.Length < 2000)
                    {
                        sb.AppendLine(sentence);
                    }
                    else
                    {
                        msgList.Add(sb.ToString());
                        sb.Clear();
                        sb.AppendLine(sentence);
                    }
                }
                msgList.Add(sb.ToString());
            }
            else
            {
                msgList.Add(msgFull);
            }

            await Context.Message.DeleteAsync();
            foreach (var msg in msgList)
            {
                await user.SendMessageAsync(msg);
            }
        }
    }
}