using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
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
        private readonly HttpClient _httpClient; 
        private readonly IServiceProvider _services;
        private readonly IGoogleService _googleService;
        private readonly ILoggerService _logger;
        private readonly IDiscordContextService _discordContext;

        public FlaskModule(IServiceProvider services, IOptionsMonitor<Config> config)
        {
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _httpClient = services.GetRequiredService<HttpClient>();
            _googleService = services.GetRequiredService<IGoogleService>();
            _logger = services.GetRequiredService<ILoggerService>();
            _discordContext = services.GetRequiredService<IDiscordContextService>();
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

        [MethodAdmin]
        [Command("raport")]
        [Summary("wysyła raport o flaszkach")]
        public async Task ReportFlask()
        {
            var googleSettings = _config.CurrentValue.GoogleSettings;        // gets curent value of GoogleSettings
            var channel = (SocketTextChannel)_discord.GetGuild(_config.CurrentValue.MyGuildId).GetChannel(_config.CurrentValue.FlaskChannelId);
            
            var reportDate = await _googleService.ReadCellAsync(googleSettings.FlaskData.ReportDateAddres);
            if (DateTime.TryParseExact(reportDate, @"MM/dd/yy (hh:mm tt)", new CultureInfo("en-US"), DateTimeStyles.None, out var reportDateResoult))
            {
                var flaskData = await _googleService.ReadDataAsync<FlaskModel>(googleSettings.FlaskData.ReportValuesAddres);
                
                var sb = new StringBuilder();
                sb.AppendLine($"[Ostatnia aktualizacja: {reportDateResoult:dd.MM.yyyy HH:mm}]".Decorate(Decorator.Bold));
                
                if (flaskData.Any(x => x.FlaskCount < 0))
                {
                    // build ref string to algin text
                    var strRef = StringExtensions.Empty(flaskData.Where(x => x.FlaskCount < 0).Max(x => x.WowNick.Length) + 6);
                    
                    sb.AppendLine("Poniższe osoby proszone są o uzupełnienie flaszek:");
                    sb.AppendLine(string.Empty);
                    foreach (var item in flaskData.Where(x => x.FlaskCount < 0).OrderBy(x => x.FlaskCount))
                        sb.AppendLine($"{StringExtensions.AlginToRef(item.WowNick, item.FlaskCount.ToString(), strRef).Decorate(Decorator.Inline_code)}\t{_discordContext.MentionTag(item.DiscordTag)}");
                }

                var chartResponse = await _httpClient.GetAsync(googleSettings.ReportChartUrl);
                var filePath = Path.Combine( Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Guid.NewGuid() + ".png");
                await using (var fs = new FileStream(filePath, FileMode.CreateNew))
                {
                    await chartResponse.Content.CopyToAsync(fs);
                }

                await channel.SendFileAsync(filePath, sb.ToString());
                File.Delete(filePath);    // cleanup
            }
            else
            {
                await Context.Channel.SendMessageAsync("Wystąpił problem z bazą flaszek, skontaktuj się z administratorem");
                await _logger.WriteLog($"Can't parse DateTime in GooglSheets: {(string.IsNullOrEmpty(reportDate) ? "null" : reportDate)}");
            }
        }
    }
}