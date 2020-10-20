using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DGSearchBot.Commands
{
    public class FunCommands : BaseCommandModule
    {
        [Command("ping")]
        [Description("Returns Pong")]
        public async Task Ping(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Pong\n").ConfigureAwait(false);
        }

        [Command("response")]
        [Description("")]
        public async Task Response(CommandContext ctx)
        {
            var interact = ctx.Client.GetInteractivity();

            var message = await interact.WaitForMessageAsync(x => x.Channel == ctx.Channel).ConfigureAwait(false);

            await ctx.Channel.SendMessageAsync(message.Result.Content);
        }
    }
}
