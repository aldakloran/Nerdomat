using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nerdomat.Interfaces;
using Nerdomat.Models;
using Nerdomat.Services;
using Nerdomat.Tools;
namespace Nerdomat.Modules
{
    [ModuleActive(true)]
    [ModuleName("Flaszki")]
    public class FlaskModule : ModuleBase<SocketCommandContext>
    {
        private readonly IOptionsMonitor<Config> _config;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;
        private readonly IGoogleService _googleService;
        private readonly ILoggerService _logger;

        public FlaskModule(IServiceProvider services, IOptionsMonitor<Config> config)
        {
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _googleService = services.GetRequiredService<IGoogleService>();
            _logger = services.GetRequiredService<ILoggerService>();
            _services = services;
            _config = config;
        }

        private string FlaskGrammaVariety(int count)
        {
            var number = Math.Abs(count);
            var lastNumber = number % 10;
            
            if (number == 1)
                return "flaszke";

            if((number > 20 || number < 10) && (lastNumber == 2 || lastNumber == 3 || lastNumber == 4))
                return "flaszki";

            return "flaszek";
        }


        [Command("flaszka")]
        [Summary("wyśietla ilość flaszek nerda")]
        public async Task UserFlask()
        {
            var userDiscordTag = $"{Context.User.Username}#{Context.User.Discriminator}";    // create full DiscordTag
            var googleSettings = _config.CurrentValue.GoogleSettings;        // gets curent value of GoogleSettings
            
            var reportDate = await _googleService.ReadCellAsync(googleSettings.FlaskData.ReportDateAddres);
            if (DateTime.TryParseExact(reportDate, @"MM/dd/yy (hh:mm tt)", new CultureInfo("en-US"), DateTimeStyles.None, out var reportDateResoult))
            {
                var flaskData = await _googleService.ReadDataAsync<FlaskModel>(googleSettings.FlaskData.ReportValuesAddres);
                var userData = flaskData.FirstOrDefault(x => string.Equals(x.DiscordTag, userDiscordTag, StringComparison.OrdinalIgnoreCase));
                if (userData != null)
                {
                    var sb = new StringBuilder();
                    sb.AppendLine($"[Ostatnia aktualizacja: {reportDateResoult:dd.MM.yyyy HH:mm}]".Decorate(Decorator.Bold));
                    sb.Append(Context.User.Mention);
                    sb.Append($" <{userData.WowNick}> masz ");
                    sb.Append(userData.FlaskCount.ToString().Decorate(Decorator.Underline));
                    sb.Append($" {FlaskGrammaVariety(userData.FlaskCount)}");

                    if (userData.FlaskCount < 0)
                    {
                        sb.AppendLine(string.Empty);
                        sb.AppendLine(string.Empty);
                        sb.AppendLine("Jak najszybciej uzupełnij brakujące flaszki!".Decorate(Decorator.underline_bold_italics));
                    }

                    await Context.Channel.SendMessageAsync(sb.ToString());
                }
                else
                {
                    await Context.Channel.SendMessageAsync("Brak w bazie... sorry\nJeżeli jest to błąd skontaktuj się z administratorem");
                    await _logger.WriteLog($"Can't find data in GooglSheets for user {userDiscordTag}");
                }
            }
            else
            {
                await Context.Channel.SendMessageAsync("Wystąpił problem z bazą flaszek, skontaktuj się z administratorem");
                await _logger.WriteLog($"Can't parse DateTime in GooglSheets: {(string.IsNullOrEmpty(reportDate) ? "null" : reportDate)}");
            }
        }
        
    }
}