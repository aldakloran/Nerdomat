using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nerdomat.Interfaces;
using Nerdomat.Models;

namespace Nerdomat.Services
{
    public class WatchdogService : IWatchdogService
    {
        private readonly IOptionsMonitor<Config> _config;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;
        private readonly List<Discord.ConnectionState> _alloweedStates;

        private Timer _timer;
        private int _failCounter;

        public WatchdogService(IServiceProvider services, IOptionsMonitor<Config> config)
        {
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _services = services;
            _config = config;

            _alloweedStates = new List<Discord.ConnectionState>
            {
                Discord.ConnectionState.Connected
            };

            Console.WriteLine($"{GetType().Name} initialized");
        }

        public async Task InitializeAsync()
        {
            await Task.Factory.StartNew(() =>
            {
                if (_timer != null) return;

                _failCounter = 0;

                _timer = new Timer(30000);  // every 30 sec
                _timer.Elapsed += WatchdogCheck;
                _timer.Start();
            });
        }

        private async void WatchdogCheck(object sender, ElapsedEventArgs e)
        {
            //Console.WriteLine("Watchdog tick");
            if (!_alloweedStates.Contains(_discord.ConnectionState))
                _failCounter++;
            else
                _failCounter = 0;

            if (_failCounter >= _config.CurrentValue.WatchdogMaxFailLimit)
            {
                _timer.Stop();
                await RestartClient();
                _timer.Start();
            }
        }

        public async Task RestartClient()
        {
            Console.WriteLine("Watchdog reboot");
            await _discord.StopAsync();
            await _discord.StartAsync();

            _failCounter = 0;
        }
    }
}