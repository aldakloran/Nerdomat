using Discord;
using Nerdomat.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nerdomat.Tools
{
    public static class NerdChecker
    {
        public const string WowLogoUrl = "https://blznav.akamaized.net/img/games/logo-wow-3dd2cfe06df74407.png";
        public const string ArmoryUrl = "https://worldofwarcraft.com/en-gb/character/eu/burning-legion/";

        public static Embed GetNerdArmory(FlaskModel flaskModel)
        {
            if (flaskModel == null) return null;

            var url = ArmoryUrl + flaskModel.WowNick.ToLower();

            var newEmbed = new EmbedBuilder
            {
                Color = Color.Red,
                Author = new EmbedAuthorBuilder
                {
                    IconUrl = WowLogoUrl,
                    Url = url,
                    Name = $"{flaskModel.WowNick} Armory"
                }
            };

            return newEmbed.Build();
        }
    }
}
