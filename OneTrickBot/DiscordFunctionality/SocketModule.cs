using Discord;
using Discord.Commands;

namespace OneTrickBot.DiscordFunctionality
{
    public abstract class SocketModule : ModuleBase<SocketCommandContext>
    {
        public IGuildUser GuildUser => Context.Guild?.GetUser(Context.User.Id);
        public string AuthorUsername => Context.User.Username;
        public string AuthorNickname => GuildUser?.Nickname;
        public string AuthorNicknameOrUsername => AuthorNickname ?? AuthorUsername;
    }
}
