using System;
using System.Collections.Generic;
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
    [ModuleName("Raider.io")]
    public class RaiderIoModule : ModuleBase<SocketCommandContext>
    {
        private readonly IOptionsMonitor<Config> _config;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;
        private readonly IRaiderIoService _rio;

        private readonly List<Color> _colors = new List<Color> { Color.Blue, Color.Green, Color.Red, Color.Orange, Color.Purple, Color.LightGrey };

        public RaiderIoModule(IServiceProvider services, IOptionsMonitor<Config> config)
        {
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _rio = services.GetRequiredService<IRaiderIoService>();
            _services = services;
            _config = config;
        }

        [Command("rio")]
        [Summary("Podsumowanie z Raider.io")]
        public async Task RioDataGet(string name)
        {
            var rioData = await _rio.ProfileGet(name);
            if (rioData != null)
            {
                var ranks = new EmbedFieldBuilder
                {
                    IsInline = true,
                    Name = "Overall",
                    Value = $"World: {rioData.MythicPlusRanks.Overall.World}\nRegion: {rioData.MythicPlusRanks.Overall.Region}\nRealm: {rioData.MythicPlusRanks.Overall.Realm}"
                };


                var dps = new EmbedFieldBuilder();
                var heal = new EmbedFieldBuilder();
                var tank = new EmbedFieldBuilder();


                if (rioData.MythicPlusRanks.Dps != null)
                {
                    dps.IsInline = true;
                    dps.Name = "Dps";
                    dps.Value = $"World: {rioData.MythicPlusRanks.Dps.World}\nRegion: {rioData.MythicPlusRanks.Dps.Region}\nRealm: {rioData.MythicPlusRanks.Dps.Realm}";
                }

                if (rioData.MythicPlusRanks.Healer != null)
                {
                    heal.IsInline = true;
                    heal.Name = "Heal";
                    heal.Value = $"World: {rioData.MythicPlusRanks.Healer.World}\nRegion: {rioData.MythicPlusRanks.Healer.Region}\nRealm: {rioData.MythicPlusRanks.Healer.Realm}";
                }

                if (rioData.MythicPlusRanks.Tank != null)
                {
                    tank.IsInline = true;
                    tank.Name = "Tank";
                    tank.Value = $"World: {rioData.MythicPlusRanks.Tank.World}\nRegion: {rioData.MythicPlusRanks.Tank.Region}\nRealm: {rioData.MythicPlusRanks.Tank.Realm}";
                }

                var footer = new EmbedFooterBuilder
                {
                    Text = $"[{DateTime.Now:yyyy-MM-dd  HH:mm:ss}]"
                };

                var newEmbed = new EmbedBuilder
                {
                    Color = _colors.OrderBy(x => Guid.NewGuid()).First(),
                    ThumbnailUrl = rioData.ThumbnailUrl,//CategoryUrl,//LogoUrl,
                    Url = rioData.ProfileUrl,
                    Title = $"{name}  {rioData.Gear.ItemLevelEquipped} ilvl  {rioData.MythicPlusScoresBySeason.FirstOrDefault()?.Scores.All} rio".Decorate(Decorator.Underline_bold),
                    Description = $"{rioData.Race} {rioData.Class} {rioData.Covenant.Name} ({rioData.Covenant.RenownLevel})  {rioData.RaidProgression.CastleNathria.Summary.Decorate(Decorator.Bold)}",
                    Footer = footer
                };

                if (rioData.ActiveSpecRole == "HEALING")
                {
                    newEmbed.Fields.Add(heal);
                }
                else if (rioData.ActiveSpecRole == "TANK")
                {
                    newEmbed.Fields.Add(tank);
                }
                else
                {
                    newEmbed.Fields.Add(dps);
                }

                newEmbed.Fields.Add(ranks);

                await Context.Channel.SendMessageAsync(string.Empty, false, newEmbed.Build());
            }
            else
            {
                await Context.Channel.SendMessageAsync($"Nie znaleziono postaci {name}");
            }
        }
    }
}