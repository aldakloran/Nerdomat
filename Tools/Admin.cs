using System.Linq;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Nerdomat.Tools
{
    public static class Admin
    {
        public static bool IsAdmin(this SocketUser user1)
        {
            var user = user1 as SocketGuildUser;
            return user?.GuildPermissions.Administrator ?? false;
        }

        public static bool IsAdmin(this IUser user1)
        {
            var user = user1 as SocketGuildUser;
            return user?.GuildPermissions.Administrator ?? false;
        }

        public static bool OnyAdminCommand(this CommandInfo ci)
        {
            return ci.Attributes.Any(x => x.GetType() == typeof(ModuleAdmin));
        }
    }
}