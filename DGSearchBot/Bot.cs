using DGSearchBot.Commands;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DGSearchBot
{
    public class Bot
    {
        public DiscordClient Client { get; private set; }
        public InteractivityExtension Interact { get; private set; }
        public CommandsNextExtension Commands { get; private set; }

        public async Task RunASync()
        {
            var json = string.Empty;
            using (var fs = File.OpenRead("config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync().ConfigureAwait(false);

            var configJson = JsonConvert.DeserializeObject<ConfigJSON>(json);

            var config = new DiscordConfiguration
            {
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug
            };

            Client = new DiscordClient(config);

            Client.Ready += OnClientReady;

            Client.UseInteractivity(new InteractivityConfiguration
            {
                Timeout = TimeSpan.FromSeconds(30)
            });

            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { configJson.Prefix },
                EnableMentionPrefix = true,
                EnableDms = false
            };

            Commands = Client.UseCommandsNext(commandsConfig);

            Commands.RegisterCommands<PrimaryCommands>();

            await Client.ConnectAsync();

            await Task.Delay(-1);
        }

        private Task OnClientReady(Object sender, ReadyEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}
