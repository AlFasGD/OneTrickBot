using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using OneTrickBot.Configuration;
using OneTrickBot.DiscordFunctionality.Attributes;
using OneTrickBot.Extensions;

namespace OneTrickBot.DiscordFunctionality
{
    public class BotConfigModule : SocketModule
    {
        [Command("prefix")]
        [Summary("Displays the current prefix for this bot on this server.")]
        [RequireGuildContext]
        public async Task DisplayCurrentPrefixAsync()
        {
            await Context.Channel.SendMessageAsync($"The current prefix for this server is {BotConfig.Instance.GetPrefixForGuild(Context.Guild).ToNonFormattableText()}");
        }

        [Command("prefix default")]
        [Summary("Resets the prefix for this bot on this server to the default one.")]
        [RequireGuildContext]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task ResetPrefixAsync()
        {
            BotConfig.Instance.ResetPrefixForGuild(Context.Guild);
            await Context.Channel.SendMessageAsync($"Reset the current prefix for this server to {BotConfig.DefaultPrefix.ToNonFormattableText()}");
        }

        [Command("prefix set")]
        [Summary("Sets the prefix for this bot on this server to the provided one.")]
        [RequireGuildContext]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task SetPrefixAsync
        (
            [Summary("The new prefix to set.")]
            string newPrefix
        )
        {
            BotConfig.Instance.SetPrefixForGuild(Context.Guild, newPrefix);
            await Context.Channel.SendMessageAsync($"Changed the current prefix for this server to {newPrefix.ToNonFormattableText()}");
        }
    }
}
