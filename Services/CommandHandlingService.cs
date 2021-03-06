using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Linq;
using Nerdomat.Interfaces;
using Nerdomat.Tools;

namespace Nerdomat.Services
{
    public class CommandHandlingService
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;
        private readonly ILoggerService _logger;

        public CommandHandlingService(IServiceProvider services)
        {
            _commands = services.GetRequiredService<CommandService>();
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _logger = services.GetRequiredService<ILoggerService>();
            _services = services;

            Console.WriteLine($"{GetType().Name} initialized");

            // Hook CommandExecuted to handle post-command-execution logic.
            _commands.CommandExecuted += CommandExecutedAsync;
            // Hook MessageReceived so we can process each message to see
            // if it qualifies as a command.
            _discord.MessageReceived += MessageReceivedAsync;
        }

        public async Task InitializeAsync()
        {
            // Register modules that are public and inherit ModuleBase<T>.
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            await _discord.SetGameAsync("!pomoc", @"https://github.com/aldakloran/Nerdomat", ActivityType.Streaming);
        }

        public async Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            // Ignore system messages, or messages from other bots
            if (!(rawMessage is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;

            // This value holds the offset where the prefix ends
            var argPos = 0;
            // Perform prefix check. You may want to replace this with
            // (!message.HasCharPrefix('!', ref argPos))
            // for a more traditional command format like !help.
            if (!message.HasCharPrefix(System.Diagnostics.Debugger.IsAttached ? '$' : '!', ref argPos)) return;

            var context = new SocketCommandContext(_discord, message);

            var onlyAdminCommand = _commands.Search(context, argPos).Commands
                                            .Select(x => x.Command)
                                            .All(x => x.OnyAdminCommand());

            if (onlyAdminCommand && !context.User.IsAdmin())
            {
                await context.Channel.SendMessageAsync($"{context.User.Mention} Nie masz uprawnień do tej komendy");
                return;
            }

            // Perform the execution of the command. In this method,
            // the command service will perform precondition and parsing check
            // then execute the command if one is matched.
            await _commands.ExecuteAsync(context, argPos, _services);
            // Note that normally a result will be returned by this format, but here
            // we will handle the result in CommandExecutedAsync,
        }

        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            // log command to console
            var msg = $"[{DateTime.Now} - {command.GetValueOrDefault()?.Module.Name ?? "Commands"}]: {context.User} {context.Message.Content}";
            await _logger.WriteLog(msg);

            // command is unspecified when there was a search failure (command not found); we don't care about these errors
            if (!command.IsSpecified)
                return;

            // the command was successful, we don't care about this result, unless we want to log that a command succeeded.
            if (result.IsSuccess)
                return;

            // the command failed, let's notify the user that something happened.
            await context.Channel.SendMessageAsync($"error: {result}");
            await _logger.WriteLog($"error: {result}");
        }
    }
}