﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using OneTrickBot.Configuration;
using OneTrickBot.Extensions;

namespace OneTrickBot.DiscordFunctionality
{
    public class CommandHandler
    {
        public static CommandHandler GlobalCommandHandler { get; private set; }

        public static IEnumerable<CommandInfo> AllAvailableCommands => GlobalCommandHandler.CommandService.Commands;
        public static IEnumerable<CommandInfo> AllPubliclyAvailableCommands => AllAvailableCommands.Where(c => !c.HasPrecondition<RequireOwnerAttribute>());

        public static DiscordSocketClient Client => BotClientManager.Instance.Client;
        public static DiscordRestClient RestClient => BotClientManager.Instance.RestClient;

        static CommandHandler()
        {
            // Nyun-nyun
            GlobalCommandHandler = new(new());
        }

        public CommandService CommandService { get; init; }

        public CommandHandler(CommandService service)
        {
            CommandService = service;
            Initialize();
        }

        #region Initialization
        private void Initialize()
        {
            Task.WaitAll(Task.Run(InitializeCommandHandling));
        }
        private async Task InitializeCommandHandling()
        {
            var thisAssembly = Assembly.GetExecutingAssembly();
            CommandService.AddTypeReaders(thisAssembly);
            await CommandService.AddModulesAsync(thisAssembly, null);
        }
        #endregion

        public void AddEvents(DiscordSocketClient client)
        {
            client.MessageReceived += HandleCommandAsync;
        }

        #region Handlers
        protected async Task HandleCommandAsync(SocketMessage message)
        {
            var socketMessage = message as SocketUserMessage;

            if (socketMessage?.Author.IsHuman() != true)
                return;

            var context = new SocketCommandContext(Client, socketMessage);

            var prefix = BotConfig.Instance.GetPrefixForGuild(context.Guild);

            if (!socketMessage.Content.StartsWith(prefix))
                return;

            ConsoleLogging.WriteEventWithCurrentTime($"{message.Author} sent a command:\n{socketMessage.Content}");

            ExecuteCommandAsync(context, prefix);
        }
        protected async Task ExecuteCommandAsync(ICommandContext context, string prefix)
        {
            var result = await CommandService.ExecuteAsync(context, prefix.Length, null);

            if (!result.IsSuccess)
            {
                var error = result.Error;
                Console.WriteLine(error);

                switch (error)
                {
                    case CommandError.UnknownCommand:
                        break;

                    default:
                        await context.Channel.SendMessageAsync(error switch
                        {
                            // The reason why this help message is not "{prefix}help {command}" is because there is no good way to get the full name of the command
                            CommandError.BadArgCount => $"Unexpected argument count, use `{prefix}help <command>` to get more help regarding this command.",
                            CommandError.ParseFailed => $"Failed to parse the command, use `{prefix}help` to get more help about the available commands.",
                            CommandError.UnmetPrecondition => $"Failed to execute the command, either because this is not for you, or this is not the right place to do it.",

                            CommandError.ObjectNotFound or
                            CommandError.MultipleMatches or
                            CommandError.Exception or
                            CommandError.Unsuccessful => $"Developer is bad, error was caused by his fault.\nError information: {error} - {result.ErrorReason}",

                            _ => "Unknown issue occurred.",
                        });
                        break;
                }

                if (result is ExecuteResult executionResult)
                {
                    Console.WriteLine();
                    Console.WriteLine(executionResult.Exception);
                    Console.WriteLine();
                    Console.WriteLine(executionResult.Exception.StackTrace);
                    Console.WriteLine();
                    Console.WriteLine(executionResult.Exception.Message);
                    Console.WriteLine();
                }
                else if (result is ParseResult parseResult)
                {
                    Console.WriteLine();
                    Console.WriteLine("Parameter Values");
                    if (parseResult.ParamValues != null)
                        foreach (var value in parseResult.ParamValues)
                            Console.WriteLine(value);

                    Console.WriteLine();
                    Console.WriteLine("Argument Values");
                    if (parseResult.ArgValues != null)
                        foreach (var value in parseResult.ArgValues)
                            Console.WriteLine(value);

                    Console.WriteLine();
                    Console.WriteLine($"Error Parameter: {parseResult.ErrorParameter}\n");
                    Console.WriteLine();
                }
            }
        }
        #endregion
    }
}
